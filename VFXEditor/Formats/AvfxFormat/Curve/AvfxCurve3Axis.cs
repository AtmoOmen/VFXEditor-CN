using System.Collections.Generic;
using System.IO;
using VfxEditor.AvfxFormat;
using VfxEditor.Formats.AvfxFormat.Curve;
using VfxEditor.Formats.AvfxFormat.Curve.Lines;
using static VfxEditor.AvfxFormat.Enums;

namespace VFXEditor.Formats.AvfxFormat.Curve {
    public class AvfxCurve3Axis : AvfxCurveBase {
        public readonly AvfxEnum<AxisConnect3> AxisConnectType = new( "轴连接", "ACT" );
        public readonly AvfxEnum<AxisConnect3> AxisConnectRandomType = new( "随机轴连接", "ACTR" );
        public readonly AvfxCurveData X;
        public readonly AvfxCurveData Y;
        public readonly AvfxCurveData Z;
        public readonly AvfxCurveData RX;
        public readonly AvfxCurveData RY;
        public readonly AvfxCurveData RZ;

        private readonly List<AvfxBase> Parsed;

        private readonly LineEditor LineEditor;

        public AvfxCurve3Axis( string name, string avfxName, CurveType type = CurveType.Base, bool locked = false ) : base( name, avfxName, locked ) {
            X = new( "X", "X", type );
            Y = new( "Y", "Y", type );
            Z = new( "Z", "Z", type );
            RX = new( "随机 X", "XR", type );
            RY = new( "随机 Y", "YR", type );
            RZ = new( "随机 Z", "ZR", type );

            Parsed = [
                AxisConnectType,
                AxisConnectRandomType,
                X,
                Y,
                Z,
                RX,
                RY,
                RZ
            ];

            LineEditor = new( name, [
                new( "Value", [X, Y, Z], AxisConnectType ),
                new( "Random", [RX, RY, RZ], AxisConnectRandomType )
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
