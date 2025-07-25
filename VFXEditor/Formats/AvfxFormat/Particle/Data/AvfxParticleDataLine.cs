using VFXEditor.Formats.AvfxFormat.Curve;

namespace VfxEditor.AvfxFormat {
    public class AvfxParticleDataLine : AvfxDataWithParameters {
        public readonly AvfxInt LineCount = new( "线数", "LnCT" );
        public readonly AvfxCurve1Axis Length = new( "长度", "Len" );
        public readonly AvfxCurve1Axis LengthRandom = new( "随机长度", "LenR" );
        public readonly AvfxCurveColor ColorBegin = new( name: "起始颜色", "ColB" );
        public readonly AvfxCurveColor ColorEnd = new( name: "结束颜色", "ColE" );

        public AvfxParticleDataLine() : base() {
            Parsed = [
                LineCount,
                Length,
                LengthRandom,
                ColorBegin,
                ColorEnd
            ];

            ParameterTab.Add( LineCount );

            Tabs.Add( Length );
            Tabs.Add( LengthRandom );
            Tabs.Add( ColorBegin );
            Tabs.Add( ColorEnd );
        }
    }
}
