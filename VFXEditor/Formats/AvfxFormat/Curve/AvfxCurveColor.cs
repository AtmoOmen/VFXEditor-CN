using ImGuiNET;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Formats.AvfxFormat.Curve;

namespace VfxEditor.AvfxFormat {
    public class AvfxCurveColor : AvfxCurveBase {
        public readonly AvfxCurve RGB = new( "RGB", "RGB", type: CurveType.Color );
        public readonly AvfxCurve A = new( "A", "A" );
        public readonly AvfxCurve SclR = new( "红", "SclR" );
        public readonly AvfxCurve SclG = new( "绿", "SclG" );
        public readonly AvfxCurve SclB = new( "蓝", "SclB" );
        public readonly AvfxCurve SclA = new( "透明度", "SclA" );
        public readonly AvfxCurve Bri = new( "亮度", "Bri" );
        public readonly AvfxCurve RanR = new( "随机红色", "RanR" );
        public readonly AvfxCurve RanG = new( "随机绿色", "RanG" );
        public readonly AvfxCurve RanB = new( "随机蓝色", "RanB" );
        public readonly AvfxCurve RanA = new( "随机透明度", "RanA" );
        public readonly AvfxCurve RBri = new( "随机亮度", "RBri" );

        private readonly List<AvfxCurveBase> Curves;

        public AvfxCurveColor( string name, string avfxName = "Col", bool locked = false ) : base( name, avfxName, locked ) {
            Curves = [
                RGB,
                A,
                SclR,
                SclG,
                SclB,
                SclA,
                Bri,
                RanR,
                RanG,
                RanB,
                RanA,
                RBri
            ];
        }

        public override void ReadContents( BinaryReader reader, int size ) => ReadNested( reader, Curves, size );

        public override void WriteContents( BinaryWriter writer ) => WriteNested( writer, Curves );

        protected override IEnumerable<AvfxBase> GetChildren() {
            foreach( var item in Curves ) yield return item;
        }

        protected override void DrawBody() {
            DrawUnassignedCurves( Curves );
            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );
            DrawAssignedCurves( Curves );
        }
    }
}
