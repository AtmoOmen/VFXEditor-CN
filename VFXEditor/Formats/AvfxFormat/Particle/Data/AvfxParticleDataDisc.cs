using VFXEditor.Formats.AvfxFormat.Curve;

namespace VfxEditor.AvfxFormat {
    public class AvfxParticleDataDisc : AvfxDataWithParameters {
        public readonly AvfxInt PartsCount = new( "组件数量", "PrtC" );
        public readonly AvfxInt PartsCountU = new( "水平组件数量", "PCnU" );
        public readonly AvfxInt PartsCountV = new( "垂直组件数量", "PCnV" );
        public readonly AvfxFloat PointIntervalFactoryV = new( "垂直点间隔因子", "PIFU" );
        public readonly AvfxCurve1Axis Angle = new( "角度", "Ang", CurveType.Angle );
        public readonly AvfxCurve1Axis AngleRandom = new( "角度随机值", "AngR", CurveType.Angle );
        public readonly AvfxCurve1Axis HeightBeginInner = new( "内部起始高度", "HBI" );
        public readonly AvfxCurve1Axis HeightBeginInnerRandom = new( "随机内部起始高度", "HBIR" );
        public readonly AvfxCurve1Axis HeightEndInner = new( "内部结束高度", "HEI" );
        public readonly AvfxCurve1Axis HeightEndInnerRandom = new( "随机内部结束高度", "HEIR" );
        public readonly AvfxCurve1Axis HeightBeginOuter = new( "外部开始高度", "HBO" );
        public readonly AvfxCurve1Axis HeightBeginOuterRandom = new( "随机外部开始高度", "HBOR" );
        public readonly AvfxCurve1Axis HeightEndOuter = new( "外部结束高度", "HEO" );
        public readonly AvfxCurve1Axis HeightEndOuterRandom = new( "随机外部结束高度", "HEOR" );
        public readonly AvfxCurve1Axis WidthBegin = new( "起始宽度", "WB" );
        public readonly AvfxCurve1Axis WidthBeginRandom = new( "随机起始宽度", "WBR" );
        public readonly AvfxCurve1Axis WidthEnd = new( "结束宽度", "WE" );
        public readonly AvfxCurve1Axis WidthEndRandom = new( "随机结束宽度", "WER" );
        public readonly AvfxCurve1Axis RadiusBegin = new( "半径起始点", "RB" );
        public readonly AvfxCurve1Axis RadiusBeginRandom = new( "随机半径起始点", "RBR" );
        public readonly AvfxCurve1Axis RadiusEnd = new( "半径结束点", "RE" );
        public readonly AvfxCurve1Axis RadiusEndRandom = new( "随机半径结束点", "RER" );
        public readonly AvfxCurveColor ColorEdgeInner = new( name: "内部边缘颜色", "CEI" );
        public readonly AvfxCurveColor ColorEdgeOuter = new( name: "外部边缘颜色", "CEO" );
        public readonly AvfxInt SS = new( "缩放比例", "SS" );

        public AvfxParticleDataDisc() : base() {
            Parsed = [
                PartsCount,
                PartsCountU,
                PartsCountV,
                PointIntervalFactoryV,
                Angle,
                AngleRandom,
                HeightBeginInner,
                HeightBeginInnerRandom,
                HeightEndInner,
                HeightEndInnerRandom,
                HeightBeginOuter,
                HeightBeginOuterRandom,
                HeightEndOuter,
                HeightEndOuterRandom,
                WidthBegin,
                WidthBeginRandom,
                WidthEnd,
                WidthEndRandom,
                RadiusBegin,
                RadiusBeginRandom,
                RadiusEnd,
                RadiusEndRandom,
                ColorEdgeInner,
                ColorEdgeOuter,
                SS
            ];

            ParameterTab.Add( PartsCount );
            ParameterTab.Add( PartsCountU );
            ParameterTab.Add( PartsCountV );
            ParameterTab.Add( PointIntervalFactoryV );
            ParameterTab.Add( SS );

            Tabs.Add( Angle );
            Tabs.Add( AngleRandom );
            Tabs.Add( HeightBeginInner );
            Tabs.Add( HeightBeginInnerRandom );
            Tabs.Add( HeightEndInner );
            Tabs.Add( HeightEndInnerRandom );
            Tabs.Add( HeightBeginOuter );
            Tabs.Add( HeightBeginOuterRandom );
            Tabs.Add( HeightEndOuter );
            Tabs.Add( HeightEndOuterRandom );
            Tabs.Add( WidthBegin );
            Tabs.Add( WidthBeginRandom );
            Tabs.Add( WidthEnd );
            Tabs.Add( WidthEndRandom );
            Tabs.Add( RadiusBegin );
            Tabs.Add( RadiusBeginRandom );
            Tabs.Add( RadiusEnd );
            Tabs.Add( RadiusEndRandom );
            Tabs.Add( ColorEdgeInner );
            Tabs.Add( ColorEdgeOuter );
        }
    }
}
