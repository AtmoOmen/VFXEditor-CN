using VFXEditor.Formats.AvfxFormat.Curve;

namespace VfxEditor.AvfxFormat {
    public class AvfxParticleDataDecalRing : AvfxDataWithParameters {
        public readonly AvfxCurve1Axis Width = new( "宽度", "WID" );
        public readonly AvfxCurve1Axis WidthRandom = new( "随机宽度", "WIDR" );
        public readonly AvfxFloat ScalingScale = new( "缩放比例", "SS" );
        public readonly AvfxFloat RingFan = new( "环形扇叶排布", "RF" );
        public readonly AvfxInt DDTT = new( "DDTT", "DDTT" );

        public AvfxParticleDataDecalRing() : base() {
            Parsed = [
                Width,
                WidthRandom,
                ScalingScale,
                RingFan,
                DDTT
            ];

            ParameterTab.Add( ScalingScale );
            ParameterTab.Add( RingFan );
            ParameterTab.Add( DDTT );

            Tabs.Add( Width );
            Tabs.Add( WidthRandom );
        }
    }
}
