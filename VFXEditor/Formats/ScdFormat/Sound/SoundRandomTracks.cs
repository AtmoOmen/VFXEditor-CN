using Dalamud.Interface.Utility.Raii;
using ImGuiNET;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Parsing;
using VfxEditor.Parsing.Int;
using VfxEditor.Ui.Components.Tables;
using VfxEditor.Ui.Interfaces;

namespace VfxEditor.ScdFormat {
    public class SoundRandomTracks {
        public readonly List<RandomTrackInfo> Entries = [];
        private readonly CommandTable<RandomTrackInfo> EntryTable;

        public readonly ParsedInt CycleInterval = new( "循环间隔" );
        public readonly ParsedShort CycleNumPlayTrack = new( "循环播放音轨" );
        public readonly ParsedShort CycleRange = new( "循环范围" );

        public SoundRandomTracks() {
            EntryTable = new( "条目", true, false, Entries, [
                ( "轨道索引", ImGuiTableColumnFlags.None, -1 ),
                ( "音频索引", ImGuiTableColumnFlags.None, -1 ),
                ( "限制", ImGuiTableColumnFlags.None, -1 ),
            ],
            () => new() );
        }

        public void Read( BinaryReader reader, SoundType type, byte trackCount ) {
            for( var i = 0; i < trackCount; i++ ) {
                var newTrack = new RandomTrackInfo();
                newTrack.Read( reader );
                Entries.Add( newTrack );
            }

            if( type == SoundType.Cycle ) {
                CycleInterval.Read( reader );
                CycleNumPlayTrack.Read( reader );
                CycleRange.Read( reader );
            }
        }

        public void Write( BinaryWriter writer, SoundType type ) {
            Entries.ForEach( x => x.Write( writer ) );

            if( type == SoundType.Cycle ) {
                CycleInterval.Write( writer );
                CycleNumPlayTrack.Write( writer );
                CycleRange.Write( writer );
            }
        }

        public void Draw( SoundType type ) {
            using var _ = ImRaii.PushId( "Tracks" );

            if( type == SoundType.Cycle ) {
                CycleInterval.Draw();
                CycleNumPlayTrack.Draw();
                CycleRange.Draw();
            }

            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 3 );

            EntryTable.Draw();
        }
    }

    public class RandomTrackInfo : IUiItem {
        public readonly SoundTrackInfo Track = new();
        public readonly ParsedShort2 Limit = new( "##Limit" );

        public void Read( BinaryReader reader ) {
            Track.Read( reader );
            Limit.Read( reader );
        }

        public void Write( BinaryWriter writer ) {
            Track.Write( writer );
            Limit.Write( writer );
        }

        public void Draw() {
            Track.Draw();

            ImGui.TableNextColumn();
            ImGui.SetNextItemWidth( 150 );
            Limit.Draw();
        }
    }
}
