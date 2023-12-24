using ImGuiNET;
using Dalamud.Interface.Utility.Raii;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Ui.Interfaces;
using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxParticleUvSet : AvfxSelectableItem {
        public readonly AvfxEnum<TextureCalculateUV> CalculateUVType = new( "Calculate UV", "CUvT" );
        public readonly AvfxCurve2Axis Scale = new( "Scale", "Scl" );
        public readonly AvfxCurve2Axis Scroll = new( "Scroll", "Scr" );
        public readonly AvfxCurve Rot = new( "Rotation", "Rot", CurveType.Angle );
        public readonly AvfxCurve RotRandom = new( "Rotation Random", "RotR", CurveType.Angle );

        private readonly List<AvfxBase> Parsed;
        private readonly List<AvfxItem> Curves;
        private readonly List<IUiItem> Display;

        public AvfxParticleUvSet() : base( "UvSt" ) {
            Parsed = new() {
                CalculateUVType,
                Scale,
                Scroll,
                Rot,
                RotRandom
            };

            Display = new() {
                CalculateUVType
            };

            Curves = new() {
                Scale,
                Scroll,
                Rot,
                RotRandom
            };
        }

        public override void ReadContents( BinaryReader reader, int size ) => ReadNested( reader, Parsed, size );

        public override void WriteContents( BinaryWriter writer ) => WriteNested( writer, Parsed );

        protected override IEnumerable<AvfxBase> GetChildren() {
            foreach( var item in Parsed ) yield return item;
        }

        public override void Draw() {
            using var _ = ImRaii.PushId( "UV" );

            DrawItems( Display );
            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );
            DrawNamedItems( Curves );
        }

        public override string GetDefaultText() => $"UV {GetIdx()}";
    }
}
