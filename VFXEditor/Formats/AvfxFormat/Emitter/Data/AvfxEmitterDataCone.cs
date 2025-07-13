using VFXEditor.Formats.AvfxFormat.Curve;
using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxEmitterDataCone : AvfxDataWithParameters {
        public readonly AvfxEnum<RotationOrder> RotationOrderType = new( "旋转顺序", "ROT" );
        public readonly AvfxCurve1Axis AX = new( "X 轴角度", "AnX", CurveType.Angle );
        public readonly AvfxCurve1Axis AY = new( "Y 轴角度", "AnY", CurveType.Angle );
        public readonly AvfxCurve1Axis AZ = new( "Angle Z", "AnZ", CurveType.Angle );
        public readonly AvfxCurve1Axis AXR = new( "Angle X Random", "AnXR", CurveType.Angle );
        public readonly AvfxCurve1Axis AYR = new( "Angle Y Random", "AnYR", CurveType.Angle );
        public readonly AvfxCurve1Axis AZR = new( "Angle Z Random", "AnZR", CurveType.Angle );
        public readonly AvfxCurve1Axis InnerSize = new( "内尺寸", "InS" );
        public readonly AvfxCurve1Axis InnerSizeRandom = new( "随机内尺寸", "InSR" );
        public readonly AvfxCurve1Axis OuterSize = new( "外尺寸", "OuS" );
        public readonly AvfxCurve1Axis OuterSizeRandom = new( "随机外尺寸", "OuSR" );
        public readonly AvfxCurve1Axis InjectionSpeed = new( "注入速度", "IjS" );
        public readonly AvfxCurve1Axis InjectionSpeedRandom = new( "随机注入速度", "IjSR" );
        public readonly AvfxCurve1Axis InjectionAngle = new( "注入角度", "IjA", CurveType.Angle );
        public readonly AvfxCurve1Axis InjectionAngleRandom = new( "随机注入角度", "IjAR", CurveType.Angle );

        public AvfxEmitterDataCone() : base() {
            Parsed = [
                RotationOrderType,
                AX,
                AY,
                AZ,
                AXR,
                AYR,
                AZR,
                InnerSize,
                InnerSizeRandom,
                OuterSize,
                OuterSizeRandom,
                InjectionSpeed,
                InjectionSpeedRandom,
                InjectionAngle,
                InjectionAngleRandom,
            ];

            ParameterTab.Add( RotationOrderType );

            Tabs.Add( AX );
            Tabs.Add( AY );
            Tabs.Add( AZ );
            Tabs.Add( AXR );
            Tabs.Add( AYR );
            Tabs.Add( AZR );
            Tabs.Add( InnerSize );
            Tabs.Add( InnerSizeRandom );
            Tabs.Add( OuterSize );
            Tabs.Add( OuterSizeRandom );
            Tabs.Add( InjectionSpeed );
            Tabs.Add( InjectionSpeedRandom );
            Tabs.Add( InjectionAngle );
            Tabs.Add( InjectionAngleRandom );
        }
    }
}
