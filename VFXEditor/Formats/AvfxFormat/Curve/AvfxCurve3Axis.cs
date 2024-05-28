using ImGuiNET;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Formats.AvfxFormat.Curve;
using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxCurve3Axis : AvfxCurveBase {
        public readonly AvfxEnum<AxisConnect> AxisConnectType = new( "轴连接", "ACT" );
        public readonly AvfxEnum<RandomType> AxisConnectRandomType = new( "随机轴连接", "ACTR" );
        public readonly AvfxCurve X;
        public readonly AvfxCurve Y;
        public readonly AvfxCurve Z;
        public readonly AvfxCurve RX;
        public readonly AvfxCurve RY;
        public readonly AvfxCurve RZ;

        private readonly List<AvfxBase> Parsed;
        private readonly List<AvfxCurveBase> Curves;

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

            Curves = [
                X,
                Y,
                Z,
                RX,
                RY,
                RZ
            ];
        }

        public override void ReadContents( BinaryReader reader, int size ) => ReadNested( reader, Parsed, size );

        public override void WriteContents( BinaryWriter writer ) => WriteNested( writer, Parsed );

        protected override IEnumerable<AvfxBase> GetChildren() {
            foreach( var item in Parsed ) yield return item;
        }

        protected override void DrawBody() {
            DrawUnassignedCurves( Curves );
            AxisConnectType.Draw();
            AxisConnectRandomType.Draw();
            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );
            DrawAssignedCurves( Curves );
        }
    }
}
