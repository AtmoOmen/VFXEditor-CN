﻿using ImGuiNET;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Parsing;
using VfxEditor.Parsing.Int;
using VfxEditor.Ui.Components;
using VfxEditor.Ui.Interfaces;
using VfxEditor.Utils;

namespace VfxEditor.SklbFormat.Data {
    public class SklbNewData : SklbData {
        private readonly int DataSize;
        private readonly List<ParsedBase> Parsed;
        private readonly List<SklbDataParent> Parents = [];
        private readonly CommandListView<SklbDataParent> ParentsView;

        public SklbNewData( BinaryReader reader ) {
            DataSize = reader.ReadInt32();
            HavokOffset = reader.ReadInt32();

            Parsed = [
                new ParsedShort( "骨骼连接索引" ),
                new ParsedReserve( 2 ), // Padding
                Id
            ];
            Parsed.ForEach( x => x.Read( reader ) );

            var numParents = ( DataSize - 0x18 ) / 0x04;
            for( var i = 0; i < numParents; i++ ) {
                Parents.Add( new( reader ) );
            }

            ParentsView = new( Parents, () => new() );

            FileUtils.PadTo( reader, 16 );
        }

        public override long Write( BinaryWriter writer ) {
            writer.Write( DataSize );
            var havokOffset = writer.BaseStream.Position;
            writer.Write( 0 ); // placeholder

            Parsed.ForEach( x => x.Write( writer ) );
            Parents.ForEach( x => x.Write( writer ) );

            FileUtils.PadTo( writer, 16 );

            return havokOffset;
        }

        public override void Draw() {
            ImGui.TextDisabled( "头版本 [新]" );
            Parsed.ForEach( x => x.Draw() );

            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 2 );
            ImGui.Separator();
            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 2 );

            ImGui.TextDisabled( "父级" );
            ParentsView.Draw();
        }
    }

    public class SklbDataParent : IUiItem {
        public readonly ParsedShort2 Parent = new( "##Parent" );

        public SklbDataParent() { }

        public SklbDataParent( BinaryReader reader ) => Parent.Read( reader );

        public void Write( BinaryWriter writer ) => Parent.Write( writer );

        public void Draw() => Parent.Draw();
    }
}
