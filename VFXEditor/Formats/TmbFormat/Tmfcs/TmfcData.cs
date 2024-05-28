﻿using ImGuiNET;
using Dalamud.Interface.Utility.Raii;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Parsing;
using VfxEditor.Ui.Interfaces;

namespace VfxEditor.TmbFormat.Tmfcs {
    public class TmfcData : IUiItem {
        public readonly TmbFile File;

        public readonly ParsedUInt Unk1 = new( "未知 1" );
        public readonly ParsedUInt Unk2 = new( "未知 2", size: 1 );
        public readonly ParsedUInt Unk3 = new( "未知 3", size: 1 );
        public readonly ParsedUInt Unk4 = new( "未知 4", size: 1 );
        public readonly ParsedUInt Unk5 = new( "未知 5", size: 1 );
        public readonly ParsedUInt Unk6 = new( "未知 6", size: 1 );
        public readonly ParsedUInt Unk7 = new( "未知 7", size: 1 );
        public readonly ParsedUInt Unk8 = new( "未知 8", size: 1 );
        public readonly ParsedUInt Unk9 = new( "未知 9", size: 1 );
        private readonly uint TempRowCount = 0;

        public readonly List<ParsedBase> Parsed = [];

        public readonly List<TmfcRow> Rows = [];

        public int Size => 0x10 + ( 0x18 * Rows.Count );

        public CommandManager Command => File.Command;

        // 24 bytes x count

        public TmfcData( BinaryReader reader, TmbFile file ) {
            File = file;
            Parsed.AddRange( [
                Unk1,
                Unk2,
                Unk3,
                Unk4,
                Unk5,
                Unk6,
                Unk7,
                Unk8,
                Unk9
            ] );

            Parsed.ForEach( x => x.Read( reader ) );
            TempRowCount = reader.ReadUInt32();
        }

        public void ReadRows( BinaryReader reader ) {
            for( var i = 0; i < TempRowCount; i++ ) Rows.Add( new TmfcRow( reader ) );
        }

        public void Write( BinaryWriter writer ) {
            Parsed.ForEach( x => x.Write( writer ) );
            writer.Write( Rows.Count );
        }

        public void WriteRows( BinaryWriter writer ) => Rows.ForEach( x => x.Write( writer ) );

        public void Draw() {
            Parsed.ForEach( x => x.Draw() );

            for( var idx = 0; idx < Rows.Count; idx++ ) {
                if( ImGui.CollapsingHeader( $"行 {idx}", ImGuiTreeNodeFlags.DefaultOpen ) ) {
                    using var _ = ImRaii.PushId( idx );
                    using var indent = ImRaii.PushIndent();
                    Rows[idx].Draw();
                }
            }
        }
    }
}
