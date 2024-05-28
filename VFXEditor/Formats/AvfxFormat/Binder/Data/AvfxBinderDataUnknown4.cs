namespace VfxEditor.AvfxFormat {
    public class AvfxBinderDataUnknown4 : AvfxData {
        public readonly AvfxCurve CarryOverFactor = new( "传递因子", "COF" );

        public AvfxBinderDataUnknown4() : base() {
            Parsed = [
                CarryOverFactor
            ];

            Tabs.Add( CarryOverFactor );
        }
    }
}
