using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxEffectorDataPointLight : AvfxDataWithParameters {
        public readonly AvfxCurveColor Color = new( "颜色" );
        public readonly AvfxCurve DistanceScale = new( "距离缩放", "DstS" );
        public readonly AvfxCurve3Axis Rotation = new( "旋转", "Rot", CurveType.Angle );
        public readonly AvfxCurve3Axis Position = new( "位置", "Pos" );
        public readonly AvfxEnum<PointLightAttenuation> PointLightAttenuationType = new( "点光源衰减", "Attn" );
        public readonly AvfxBool EnableShadow = new( "启用阴影", "bSdw" );
        public readonly AvfxBool EnableCharShadow = new( "启用角色阴影", "bChS" );
        public readonly AvfxBool EnableMapShadow = new( "启用地图阴影", "bMpS" );
        public readonly AvfxBool EnableMoveShadow = new( "启用移动阴影", "bMvS" );
        public readonly AvfxFloat ShadowCreateDistanceNear = new( "创建近距离效果", "SCDN" );
        public readonly AvfxFloat ShadowCreateDistanceFar = new( "创建远距离效果", "SCDF" );

        public AvfxEffectorDataPointLight() : base() {
            Parsed = [
                Color,
                DistanceScale,
                Rotation,
                Position,
                PointLightAttenuationType,
                EnableShadow,
                EnableCharShadow,
                EnableMapShadow,
                EnableMoveShadow,
                ShadowCreateDistanceNear,
                ShadowCreateDistanceFar
            ];

            ParameterTab.Add( PointLightAttenuationType );
            ParameterTab.Add( EnableShadow );
            ParameterTab.Add( EnableCharShadow );
            ParameterTab.Add( EnableMapShadow );
            ParameterTab.Add( EnableMoveShadow );
            ParameterTab.Add( ShadowCreateDistanceNear );
            ParameterTab.Add( ShadowCreateDistanceFar );

            Tabs.Add( Color );
            Tabs.Add( DistanceScale );
            Tabs.Add( Rotation );
            Tabs.Add( Position );
        }
    }
}
