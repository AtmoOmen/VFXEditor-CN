﻿using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxEmitterDataSphereModel : AvfxDataWithParameters {
        public readonly AvfxEnum<RotationOrder> RotationOrderType = new( "旋转顺序", "ROT" );
        public readonly AvfxEnum<GenerateMethod> GenerateMethodType = new( "生成方法", "GeMT" );
        public readonly AvfxInt DivideX = new( "X 轴分割", "DivX", value: 1 );
        public readonly AvfxInt DivideY = new( "Y 轴分割", "DivY", value: 1 );
        public readonly AvfxCurve Radius = new( "半径", "Rads" );
        public readonly AvfxCurve AZ = new( "Z 角度", "AnZ", CurveType.Angle );
        public readonly AvfxCurve AYR = new( "随机 Y 轴角度", "AnYR", CurveType.Angle );
        public readonly AvfxCurve InjectionSpeed = new( "注入速度", "IjS" );
        public readonly AvfxCurve InjectionSpeedRandom = new( "随机注入速度", "IjSR" );

        public AvfxEmitterDataSphereModel() : base() {
            Parsed = [
                RotationOrderType,
                GenerateMethodType,
                DivideX,
                DivideY,
                Radius,
                AZ,
                AYR,
                InjectionSpeed,
                InjectionSpeedRandom
            ];

            ParameterTab.Add( RotationOrderType );
            ParameterTab.Add( GenerateMethodType );
            ParameterTab.Add( DivideX );
            ParameterTab.Add( DivideY );

            Tabs.Add( Radius );
            Tabs.Add( AZ );
            Tabs.Add( AYR );
            Tabs.Add( InjectionSpeed );
            Tabs.Add( InjectionSpeedRandom );
        }
    }
}
