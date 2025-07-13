using VFXEditor.Formats.AvfxFormat.Curve;

namespace VfxEditor.AvfxFormat {
    public class AvfxBinderDataCamera : AvfxData {
        public readonly AvfxCurve1Axis Distance = new( "距离", "Dst" );
        public readonly AvfxCurve1Axis DistanceRandom = new( "随机距离", "DstR" );

        public AvfxBinderDataCamera() : base() {
            Parsed = [
                Distance,
                DistanceRandom
            ];

            Tabs.Add( Distance );
            Tabs.Add( DistanceRandom );
        }
    }
}
