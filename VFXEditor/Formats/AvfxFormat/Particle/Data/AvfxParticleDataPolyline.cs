using VFXEditor.Formats.AvfxFormat.Curve;
using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxParticleDataPolyline : AvfxDataWithParameters {
        public readonly AvfxEnum<LineCreateType> CreateLineType = new( "创建线型", "LnCT" );
        public readonly AvfxEnum<NotBillboardBaseAxisType> NotBillBoardBaseAxisType = new( "非广告牌基准轴", "NBBA" );
        public readonly AvfxInt BindWeaponType = new( "绑定武器类型", "BWpT" );
        public readonly AvfxInt PointCount = new( "点数量", "PnC" );
        public readonly AvfxInt PointCountCenter = new( "点数量中心", "PnCC" );
        public readonly AvfxInt PointCountEndDistortion = new( "点数量末端失真", "PnED" );
        public readonly AvfxBool UseEdge = new( "使用边缘", "bEdg" );
        public readonly AvfxBool NotBillboard = new( "无广告牌效果", "bNtB" );
        public readonly AvfxBool BindWeapon = new( "绑定武器", "BdWp" );
        public readonly AvfxBool ConnectTarget = new( "连接目标", "bCtg" );
        public readonly AvfxBool ConnectTargetReverse = new( "连接目标反转", "bCtr" );
        public readonly AvfxInt TagNumber = new( "标签编号", "TagN" );
        public readonly AvfxBool IsSpline = new( "是否为样条曲线", "bSpl" );
        public readonly AvfxBool IsLocal = new( "是否位于本地", "bLcl" );

        public readonly AvfxCurve1Axis CF = new( "CF", "CF" );
        public readonly AvfxCurve1Axis CFR = new( "随机 CF", "CFR" );
        public readonly AvfxCurve1Axis Width = new( "宽度", "Wd" );
        public readonly AvfxCurve1Axis WidthRandom = new( "随机宽度", "WdR" );
        public readonly AvfxCurve1Axis WidthBegin = new( "起始宽度", "WdB" );
        public readonly AvfxCurve1Axis WidthBeginRandom = new( "随机起始宽度", "WdBR" );
        public readonly AvfxCurve1Axis WidthCenter = new( "中心宽度", "WdC" );
        public readonly AvfxCurve1Axis WidthCenterRandom = new( "随机中心宽度", "WdCR" );
        public readonly AvfxCurve1Axis WidthEnd = new( "结束宽度", "WdE" );
        public readonly AvfxCurve1Axis WidthEndRandom = new( "随机结束宽度", "WdER" );
        public readonly AvfxCurve1Axis Length = new( "长度", "Len" );
        public readonly AvfxCurve1Axis LengthRandom = new( "随机长度", "LenR" );
        public readonly AvfxCurve1Axis Softness = new( "模糊度", "Sft" );
        public readonly AvfxCurve1Axis SoftRandom = new( "随机模糊度", "SftR" );
        public readonly AvfxCurve1Axis PnDs = new( "PnDs (未知)", "PnDs" );
        public readonly AvfxCurveColor ColorBegin = new( name: "起始颜色", "ColB" );
        public readonly AvfxCurveColor ColorCenter = new( name: "中心颜色", "ColC" );
        public readonly AvfxCurveColor ColorEnd = new( name: "结束颜色", "ColE" );
        public readonly AvfxCurveColor ColorEdgeBegin = new( name: "起始颜色渐变", "CoEB" );
        public readonly AvfxCurveColor ColorEdgeCenter = new( name: "中心颜色渐变", "CoEC" );
        public readonly AvfxCurveColor ColorEdgeEnd = new( name: "结束颜色渐变", "CoEE" );

        public AvfxParticleDataPolyline() : base() {
            Parsed = [
                CreateLineType,
                NotBillBoardBaseAxisType,
                BindWeaponType,
                PointCount,
                PointCountCenter,
                PointCountEndDistortion,
                UseEdge,
                NotBillboard,
                BindWeapon,
                ConnectTarget,
                ConnectTargetReverse,
                TagNumber,
                IsSpline,
                IsLocal,
                CF,
                CFR,
                Width,
                WidthRandom,
                WidthBegin,
                WidthBeginRandom,
                WidthCenter,
                WidthCenterRandom,
                WidthEnd,
                WidthEndRandom,
                Length,
                LengthRandom,
                Softness,
                SoftRandom,
                PnDs,
                ColorBegin,
                ColorCenter,
                ColorEnd,
                ColorEdgeBegin,
                ColorEdgeCenter,
                ColorEdgeEnd
            ];

            ParameterTab.Add( CreateLineType );
            ParameterTab.Add( NotBillBoardBaseAxisType );
            ParameterTab.Add( BindWeaponType );
            ParameterTab.Add( PointCount );
            ParameterTab.Add( PointCountCenter );
            ParameterTab.Add( PointCountEndDistortion );
            ParameterTab.Add( UseEdge );
            ParameterTab.Add( NotBillboard );
            ParameterTab.Add( BindWeapon );
            ParameterTab.Add( ConnectTarget );
            ParameterTab.Add( ConnectTargetReverse );
            ParameterTab.Add( TagNumber );
            ParameterTab.Add( IsSpline );
            ParameterTab.Add( IsLocal );

            Tabs.Add( Width );
            Tabs.Add( WidthBegin );
            Tabs.Add( WidthBeginRandom );
            Tabs.Add( WidthCenter );
            Tabs.Add( WidthCenterRandom );
            Tabs.Add( WidthEnd );
            Tabs.Add( WidthEndRandom );
            Tabs.Add( Length );
            Tabs.Add( LengthRandom );

            Tabs.Add( ColorBegin );
            Tabs.Add( ColorCenter );
            Tabs.Add( ColorEnd );
            Tabs.Add( ColorEdgeBegin );
            Tabs.Add( ColorEdgeCenter );
            Tabs.Add( ColorEdgeEnd );

            Tabs.Add( CF );
            Tabs.Add( CFR );
            Tabs.Add( Softness );
            Tabs.Add( SoftRandom );
            Tabs.Add( PnDs );
        }
    }
}
