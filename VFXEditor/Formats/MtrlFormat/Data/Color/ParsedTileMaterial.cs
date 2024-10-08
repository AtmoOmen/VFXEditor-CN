using Dalamud.Interface.Utility.Raii;
using ImGuiNET;
using System;
using System.IO;
using System.Numerics;
using VfxEditor.Parsing;

namespace VfxEditor.Formats.MtrlFormat.Data.Color {
    public class ParsedTileMaterial : ParsedInt {
        public ParsedTileMaterial( string name, int size = 4 ) : base( name, size ) { }

        public ParsedTileMaterial( string name, int value, int size = 4 ) : base( name, value, size ) { }

        public override void Read( BinaryReader reader ) {
            Value = ( int )( ( float )reader.ReadHalf() * 64f );
        }

        public override void Write( BinaryWriter writer ) {
            writer.Write( ( Half )( ( Value + 0.5f ) / 64f ) );
        }

        protected override void DrawBody() {
            var size = Math.Min( Plugin.MtrlManager.TileNormal.Count, Plugin.MtrlManager.TileDiffuse.Count );

            var text = ( Value >= 0 && Value < size ) ? $"倾斜 {Value}" : "[未知]";
            using var combo = ImRaii.Combo( Name, text );
            if( !combo ) return;

            for( var i = 0; i < size; i++ ) {
                if( ImGui.Selectable( $"倾斜 {i}", i == Value ) ) Update( i );
                if( i == Value ) ImGui.SetItemDefaultFocus();

                if( ImGui.IsItemHovered() ) {
                    ImGui.BeginTooltip();
                    var diffuse = Plugin.MtrlManager.TileDiffuse[i];
                    var normal = Plugin.MtrlManager.TileNormal[i];
                    ImGui.Image( diffuse.ImGuiHandle, new Vector2( 100, 100 ) );
                    ImGui.SameLine();
                    ImGui.Image( normal.ImGuiHandle, new Vector2( 100, 100 ) );
                    ImGui.EndTooltip();
                }
            }
        }
    }
}
