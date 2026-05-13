using Dalamud.Hooking;
using FFXIVClientStructs.FFXIV.Client.Sound;
using InteropGenerator.Runtime;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace VfxEditor.Interop {
    public unsafe partial class ResourceLoader {
        private IntPtr OverridenSound = IntPtr.Zero;
        private int OverridenSoundIdx = -1;

        // ====== PLAY SOUND =======

        public delegate IntPtr PlaySoundDelegate( IntPtr path, byte play );

        private readonly PlaySoundDelegate PlaySoundPath;

        public void PlaySound( string path, int idx ) {
            if( string.IsNullOrEmpty( path ) || idx < 0 || !Plugin.ScdManager.FileExists( path ) ) return;

            var bytes = Encoding.ASCII.GetBytes( path );
            var ptr = Marshal.AllocHGlobal( bytes.Length + 1 );
            Marshal.Copy( bytes, 0, ptr, bytes.Length );
            Marshal.WriteByte( ptr + bytes.Length, 0 );

            OverridenSound = ptr;
            OverridenSoundIdx = idx;

            PlaySoundPath( ptr, 1 );

            OverridenSound = IntPtr.Zero;
            OverridenSoundIdx = -1;

            Marshal.FreeHGlobal( ptr );
        }

        // ====== INIT SOUND =========

        public delegate SoundData* InitSoundPrototype( SoundManager* manager, CStringPointer path, float volume, uint soundIdx, uint unk1, bool unk2, SoundVolumeCategory category );

        public Hook<InitSoundPrototype> InitSoundHook { get; private set; }

        private SoundData* InitSoundDetour( SoundManager* manager, CStringPointer path, float volume, uint soundIdx, uint unk1, bool unk2, SoundVolumeCategory category ) {
            if( path.HasValue && (nint)path.Value == OverridenSound ) {
                return InitSoundHook.Original( manager, path, volume, ( uint )OverridenSoundIdx , unk1, unk2, category );
            }

            return InitSoundHook.Original( manager, path, volume, soundIdx, unk1, unk2, category );
        }
    }
}
