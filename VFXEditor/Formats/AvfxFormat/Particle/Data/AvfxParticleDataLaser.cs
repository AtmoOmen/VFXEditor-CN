using VFXEditor.Formats.AvfxFormat.Curve;

namespace VfxEditor.AvfxFormat {
    public class AvfxParticleDataLaser : AvfxData {
        public readonly AvfxCurve1Axis Length = new( "长度", "Len" );
        public readonly AvfxCurve1Axis LengthRandom = new( "随机长度", "LenR" );
        public readonly AvfxCurve1Axis Width = new( "宽度", "Wdt" );
        public readonly AvfxCurve1Axis WidthRandom = new( "随机宽度", "WdtR" );

        public AvfxParticleDataLaser() : base() {
            Parsed = [
                Length,
                LengthRandom,
                Width,
                WidthRandom
            ];

            Tabs.Add( Length );
            Tabs.Add( LengthRandom );
            Tabs.Add( Width );
            Tabs.Add( WidthRandom );
        }
    }
}
