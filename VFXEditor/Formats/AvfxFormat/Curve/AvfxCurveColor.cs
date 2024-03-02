using Dalamud.Interface.Utility.Raii;
using ImGuiNET;
using System.Collections.Generic;
using System.IO;

namespace VfxEditor.AvfxFormat {
    public class AvfxCurveColor : AvfxOptional {
        public readonly string Name;
        public readonly bool Locked;

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

        private readonly List<AvfxCurve> Curves;

        public AvfxCurveColor( string name, string avfxName = "Col", bool locked = false ) : base( avfxName ) {
            Name = name;
            Locked = locked;

            Curves = new() {
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
            };
        }

        public override void ReadContents( BinaryReader reader, int size ) => ReadNested( reader, Curves, size );

        public override void WriteContents( BinaryWriter writer ) => WriteNested( writer, Curves );

        protected override IEnumerable<AvfxBase> GetChildren() {
            foreach( var item in Curves ) yield return item;
        }

        public override void DrawUnassigned() {
            using var _ = ImRaii.PushId( Name );

            AssignedCopyPaste( Name );
            DrawAssignButton( Name, true );
        }

        public override void DrawAssigned() {
            using var _ = ImRaii.PushId( Name );

            AssignedCopyPaste( Name );
            if( !Locked && DrawUnassignButton( Name ) ) return;

            AvfxCurve.DrawUnassignedCurves( Curves );

            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );

            AvfxCurve.DrawAssignedCurves( Curves );
        }

        public override string GetDefaultText() => Name;
    }
}
