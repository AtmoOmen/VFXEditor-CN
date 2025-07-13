using VFXEditor.Formats.AvfxFormat.Curve;
using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxEmitterDataModel : AvfxDataWithParameters {
        public readonly AvfxInt ModelIdx = new( "模型索引", "MdNo", value: -1 );
        public readonly AvfxEnum<RotationOrder> RotationOrderType = new( "旋转顺序", "ROT" );
        public readonly AvfxEnum<GenerateMethod> GenerateMethodType = new( "生成方法", "GeMT" );
        public readonly AvfxCurve1Axis AX = new( "X 轴角度", "AnX", CurveType.Angle );
        public readonly AvfxCurve1Axis AY = new( "Y 轴角度", "AnY", CurveType.Angle );
        public readonly AvfxCurve1Axis AZ = new( "Z 轴角度", "AnZ", CurveType.Angle );
        public readonly AvfxCurve1Axis AXR = new( "随机 X 轴角度", "AnXR", CurveType.Angle );
        public readonly AvfxCurve1Axis AYR = new( "随机 Y 轴角度", "AnYR", CurveType.Angle );
        public readonly AvfxCurve1Axis AZR = new( "随机 Z 轴角度", "AnZR", CurveType.Angle );
        public readonly AvfxCurve1Axis InjectionSpeed = new( "注入速度", "IjS" );
        public readonly AvfxCurve1Axis InjectionSpeedRandom = new( "随机注入速度", "IjSR" );

        public readonly AvfxNodeSelect<AvfxModel> ModelSelect;

        public AvfxEmitterDataModel( AvfxEmitter emitter ) : base() {
            Parsed = [
                ModelIdx,
                RotationOrderType,
                GenerateMethodType,
                AX,
                AY,
                AZ,
                AXR,
                AYR,
                AZR,
                InjectionSpeed,
                InjectionSpeedRandom
            ];

            ParameterTab.Add( ModelSelect = new AvfxNodeSelect<AvfxModel>( emitter, "模型", emitter.NodeGroups.Models, ModelIdx ) );
            ParameterTab.Add( RotationOrderType );
            ParameterTab.Add( GenerateMethodType );

            Tabs.Add( AX );
            Tabs.Add( AY );
            Tabs.Add( AZ );
            Tabs.Add( AXR );
            Tabs.Add( AYR );
            Tabs.Add( AZR );
            Tabs.Add( InjectionSpeed );
            Tabs.Add( InjectionSpeedRandom );
        }

        public override void Enable() {
            ModelSelect.Enable();
        }

        public override void Disable() {
            ModelSelect.Disable();
        }
    }
}
