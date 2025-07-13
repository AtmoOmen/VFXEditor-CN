using VFXEditor.Formats.AvfxFormat.Curve;

namespace VfxEditor.AvfxFormat {
    public class AvfxBinderDataPoint : AvfxData {
        public readonly AvfxCurve1Axis SpringStrength = new( "弹簧强度", "SpS" );
        public readonly AvfxCurve1Axis SpringStrengthRandom = new( "随机弹簧强度", "SpSR" );

        public AvfxBinderDataPoint() : base() {
            Parsed = [
                SpringStrength,
                SpringStrengthRandom,
            ];

            Tabs.Add( SpringStrength );
            Tabs.Add( SpringStrengthRandom );
        }
    }
}
