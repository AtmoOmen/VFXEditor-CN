using System.Collections.Generic;
using System.IO;
using VfxEditor.Formats.AvfxFormat.Curve;
using VfxEditor.Formats.AvfxFormat.Curve.Lines;
using VFXEditor.Formats.AvfxFormat.Curve;

namespace VfxEditor.AvfxFormat {
    public class AvfxCurveColor : AvfxCurveBase {
        public readonly AvfxCurveData RGB = new( "RGB", "RGB", type: CurveType.Color );
        public readonly AvfxCurveData A = new( "A", "A" );
        public readonly AvfxCurveData SclR = new( "红", "SclR" );
        public readonly AvfxCurveData SclG = new( "绿", "SclG" );
        public readonly AvfxCurveData SclB = new( "蓝", "SclB" );
        public readonly AvfxCurveData SclA = new( "透明度", "SclA" );
        public readonly AvfxCurveData Bri = new( "亮度", "Bri" );
        public readonly AvfxCurveData RanR = new( "随机红色", "RanR" );
        public readonly AvfxCurveData RanG = new( "随机绿色", "RanG" );
        public readonly AvfxCurveData RanB = new( "随机蓝色", "RanB" );
        public readonly AvfxCurveData RanA = new( "随机透明度", "RanA" );
        public readonly AvfxCurveData RBri = new( "随机亮度", "RBri" );

        private readonly List<AvfxCurveBase> Parsed;

        private readonly LineEditor LineEditor;

        public AvfxCurveColor( string name, string avfxName = "Col", bool locked = false ) : base( name, avfxName, locked ) {
            Parsed = [
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

            LineEditor = new( name, [
                new( RGB ),
                new( "Brightness", [A, Bri], null ),
                new( "Scale", [SclR, SclG, SclB, SclA], null),
                new( "Random", [RanR, RanG, RanB, RanA, RBri], null)
            ] );
        }

        public override void ReadContents( BinaryReader reader, int size ) => ReadNested( reader, Parsed, size );

        public override void WriteContents( BinaryWriter writer ) => WriteNested( writer, Parsed );

        protected override IEnumerable<AvfxBase> GetChildren() {
            foreach( var item in Parsed ) yield return item;
        }

        public override void DrawBody() {
            LineEditor.Draw();
        }
    }
}
