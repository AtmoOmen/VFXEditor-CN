using VFXEditor.Formats.AvfxFormat.Curve;

namespace VfxEditor.AvfxFormat {
    public class AvfxBinderDataSpline : AvfxData {
        public readonly AvfxCurve1Axis CarryOverFactor = new( "传递因子", "COF" );
        public readonly AvfxCurve1Axis CarryOverFactorRandom = new( "随机传递因子", "COFR" );

        public AvfxBinderDataSpline() : base() {
            Parsed = [
                CarryOverFactor,
                CarryOverFactorRandom
            ];

            Tabs.Add( CarryOverFactor );
            Tabs.Add( CarryOverFactorRandom );
        }
    }
}
