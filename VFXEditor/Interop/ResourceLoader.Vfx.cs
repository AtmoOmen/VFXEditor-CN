using Dalamud.Hooking;
using FFXIVClientStructs.FFXIV.Client.Graphics.Scene;
using InteropGenerator.Runtime;
using System;
using VfxEditor.Spawn;

namespace VfxEditor.Interop {
    public unsafe partial class ResourceLoader {
        //====== STATIC ===========
        public VfxObject.Delegates.Create StaticVfxCreate;

        public delegate IntPtr StaticVfxRunDelegate( VfxObject* vfx, float a1, uint a2 );

        public StaticVfxRunDelegate StaticVfxRun;

        public delegate IntPtr StaticVfxRemoveDelegate( VfxObject* vfx );

        public StaticVfxRemoveDelegate StaticVfxRemove;

        // ======= STATIC HOOKS ========
        public Hook<VfxObject.Delegates.Create> StaticVfxCreateHook { get; private set; }

        public Hook<StaticVfxRemoveDelegate> StaticVfxRemoveHook { get; private set; }

        // ======== ACTOR =============
        public delegate VfxObject* ActorVfxCreateDelegate( string path, IntPtr a2, IntPtr a3, float a4, char a5, ushort a6, char a7 );

        public ActorVfxCreateDelegate ActorVfxCreate;

        public delegate IntPtr ActorVfxRemoveDelegate( VfxObject* vfx, char a2 );

        public ActorVfxRemoveDelegate ActorVfxRemove;

        // ======== ACTOR HOOKS =============
        public Hook<ActorVfxCreateDelegate> ActorVfxCreateHook { get; private set; }

        public Hook<ActorVfxRemoveDelegate> ActorVfxRemoveHook { get; private set; }

        // ======= TRIGGERS =============
        public delegate IntPtr VfxUseTriggerDelete( VfxObject* vfx, uint triggerId );

        public Hook<VfxUseTriggerDelete> VfxUseTriggerHook { get; private set; }

        // ==============================

        private VfxObject* StaticVfxNewDetour( CStringPointer path, CStringPointer pool ) {
            var vfx = StaticVfxCreateHook.Original( path, pool );
            Plugin.TrackerManager?.Vfx.AddStatic( vfx, $"{path}" );

            if( Plugin.Configuration?.LogVfxDebug == true ) Dalamud.Log( $"New Static: {path} {(nint)vfx:X8}" );
            return vfx;
        }

        private IntPtr StaticVfxRemoveDetour( VfxObject* vfx ) {
            VfxSpawn.InteropRemoved( vfx );
            Plugin.TrackerManager?.Vfx.RemoveStatic( vfx );
            return StaticVfxRemoveHook.Original( vfx );
        }

        private VfxObject* ActorVfxNewDetour( string path, IntPtr a2, IntPtr a3, float a4, char a5, ushort a6, char a7 ) {
            var vfx = ActorVfxCreateHook.Original( path, a2, a3, a4, a5, a6, a7 );
            Plugin.TrackerManager?.Vfx.AddActor( vfx, path );

            if( Plugin.Configuration?.LogVfxDebug == true ) Dalamud.Log( $"New Actor: {path} {(nint)vfx:X8}" );
            return vfx;
        }

        private IntPtr ActorVfxRemoveDetour( VfxObject* vfx, char a2 ) {
            VfxSpawn.InteropRemoved( vfx );
            Plugin.TrackerManager?.Vfx.RemoveActor( vfx );
            return ActorVfxRemoveHook.Original( vfx, a2 );
        }

        private IntPtr VfxUseTriggerDetour( VfxObject* vfx, uint triggerId ) {
            var timeline = VfxUseTriggerHook.Original( vfx, triggerId );

            if( Plugin.Configuration?.LogVfxTriggers == true ) Dalamud.Log( $"Trigger {triggerId} on {(nint)vfx:X8}, timeline: {timeline:X8}" );
            return timeline;
        }
    }
}
