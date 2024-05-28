using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxParticleDataWindmill : AvfxDataWithParameters {
        public readonly AvfxEnum<WindmillUVType> WindmillUVType = new( "风车 UV 映射类型", "WUvT" );

        public AvfxParticleDataWindmill() : base() {
            Parsed = [
                WindmillUVType
            ];

            ParameterTab.Add( WindmillUVType );
        }
    }
}
