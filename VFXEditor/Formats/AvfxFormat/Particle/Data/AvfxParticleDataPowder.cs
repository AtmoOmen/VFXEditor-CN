using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxParticleDataPowder : AvfxDataWithParameters {
        public readonly AvfxBool MV = new( "bMV", "bMV" );
        public readonly AvfxBool Loc = new( "bLoc", "bLoc" );
        public readonly AvfxBool IsLightning = new( "是否点亮", "bLgt" );
        public readonly AvfxEnum<DirectionalLightType> DirectionalLightType = new( "定向光源类型", "LgtT" );
        public readonly AvfxFloat CenterOffset = new( "中心偏移", "CnOf" );

        public AvfxParticleDataPowder() : base() {
            Parsed = [
                MV,
                Loc,
                IsLightning,
                DirectionalLightType,
                CenterOffset
            ];

            ParameterTab.Add( MV );
            ParameterTab.Add( Loc );
            ParameterTab.Add( DirectionalLightType );
            ParameterTab.Add( IsLightning );
            ParameterTab.Add( CenterOffset );
        }
    }
}
