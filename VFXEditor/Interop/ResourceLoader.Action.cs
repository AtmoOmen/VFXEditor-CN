using Dalamud.Hooking;
using FFXIVClientStructs.FFXIV.Client.System.Scheduler.Base;
using System;

namespace VfxEditor.Interop {
    public unsafe partial class ResourceLoader {
        // ====== PLAY ACTION =======

        public delegate ulong PlayActionPrototype( SchedulerTimeline* timeline );

        public Hook<PlayActionPrototype> PlayActionHook { get; private set; }

        private ulong PlayActionDetour( SchedulerTimeline* timeline ) {
            var ret = PlayActionHook.Original( timeline );
            Plugin.TrackerManager?.Tmb.AddAction( timeline );
            return ret;
        }
    }
}
