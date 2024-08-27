using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxEmitterDataCylinderModel : AvfxDataWithParameters {
        public readonly AvfxEnum<RotationOrder> RotationOrderType = new( "旋转顺序", "ROT" );
        public readonly AvfxEnum<GenerateMethod> GenerateMethodType = new( "生成方法", "GeMT" );
        public readonly AvfxInt DivideX = new( "X 轴分割", "DivX", value: 1 );
        public readonly AvfxInt DivideY = new( "Y 轴分割", "DivY", value: 1 );
        public readonly AvfxCurve Length = new( "长度", "Len" );
        public readonly AvfxCurve Radius = new( "半径", "Rad" );
        public readonly AvfxCurve AX = new( "X 轴角度", "AnX", CurveType.Angle );
        public readonly AvfxCurve AY = new( "Y 轴角度", "AnY", CurveType.Angle );
        public readonly AvfxCurve InjectionSpeed = new( "注入速度", "IjS" );
        public readonly AvfxCurve InjectionSpeedRandom = new( "随机注入速度", "IjSR" );

        public AvfxEmitterDataCylinderModel() : base() {
            Parsed = [
                RotationOrderType,
                GenerateMethodType,
                DivideX,
                DivideY,
                Length,
                Radius,
                AX,
                AY,
                InjectionSpeed,
                InjectionSpeedRandom
            ];

            ParameterTab.Add( RotationOrderType );
            ParameterTab.Add( GenerateMethodType );
            ParameterTab.Add( DivideX );
            ParameterTab.Add( DivideY );

            Tabs.Add( Radius );
            Tabs.Add( Length );
            Tabs.Add( AX );
            Tabs.Add( AY );
            Tabs.Add( InjectionSpeed );
            Tabs.Add( InjectionSpeedRandom );
        }
    }
}
