namespace VfxEditor.AvfxFormat {
    public class AvfxParticleDataDecal : AvfxDataWithParameters {
        public readonly AvfxFloat ScalingScale = new( "缩放比例", "SS" );
        public readonly AvfxInt DDTT = new( "DDTT", "DDTT" );

        public AvfxParticleDataDecal() : base() {
            Parsed = [
                ScalingScale,
                DDTT,
            ];

            ParameterTab.Add( ScalingScale );
            ParameterTab.Add( DDTT );
        }
    }
}
