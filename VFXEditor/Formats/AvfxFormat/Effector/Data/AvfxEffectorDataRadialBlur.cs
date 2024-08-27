using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxEffectorDataRadialBlur : AvfxDataWithParameters {
        public readonly AvfxCurve Length = new( "长度", "Len" );
        public readonly AvfxCurve Strength = new( "强度", "Str" );
        public readonly AvfxCurve Gradation = new( "渐变", "Gra" );
        public readonly AvfxCurve InnerRadius = new( "内半径", "IRad" );
        public readonly AvfxCurve OuterRadius = new( "外半径", "ORad" );
        public readonly AvfxFloat FadeStartDistance = new( "开始淡出距离", "FSDc" );
        public readonly AvfxFloat FadeEndDistance = new( "结束淡出距离", "FEDc" );
        public readonly AvfxEnum<ClipBasePoint> FadeBasePointType = new( "淡出基准点", "FaBP" );

        public AvfxEffectorDataRadialBlur() : base() {
            Parsed = [
                Length,
                Strength,
                Gradation,
                InnerRadius,
                OuterRadius,
                FadeStartDistance,
                FadeEndDistance,
                FadeBasePointType
            ];

            ParameterTab.Add( FadeStartDistance );
            ParameterTab.Add( FadeEndDistance );
            ParameterTab.Add( FadeBasePointType );

            Tabs.Add( Length );
            Tabs.Add( Strength );
            Tabs.Add( Gradation );
            Tabs.Add( InnerRadius );
            Tabs.Add( OuterRadius );
        }
    }
}
