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

        public readonly AvfxCurve CF = new( "CF (未知)", "CF" );
        public readonly AvfxCurve Width = new( "宽度", "Wd" );
        public readonly AvfxCurve WidthRandom = new( "随机宽度", "WdR" );
        public readonly AvfxCurve WidthBegin = new( "起始宽度", "WdB" );
        public readonly AvfxCurve WidthCenter = new( "中心宽度", "WdC" );
        public readonly AvfxCurve WidthEnd = new( "结束宽度", "WdE" );
        public readonly AvfxCurve Length = new( "长度", "Len" );
        public readonly AvfxCurve LengthRandom = new( "随机长度", "LenR" );
        public readonly AvfxCurve Softness = new( "模糊度", "Sft" );
        public readonly AvfxCurve SoftRandom = new( "随机模糊度", "SftR" );
        public readonly AvfxCurve PnDs = new( "PnDs (未知)", "PnDs" );
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
                Width,
                WidthRandom,
                WidthBegin,
                WidthCenter,
                WidthEnd,
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
            Tabs.Add( WidthCenter );
            Tabs.Add( WidthEnd );
            Tabs.Add( Length );
            Tabs.Add( LengthRandom );

            Tabs.Add( ColorBegin );
            Tabs.Add( ColorCenter );
            Tabs.Add( ColorEnd );
            Tabs.Add( ColorEdgeBegin );
            Tabs.Add( ColorEdgeCenter );
            Tabs.Add( ColorEdgeEnd );

            Tabs.Add( CF );
            Tabs.Add( Softness );
            Tabs.Add( SoftRandom );
            Tabs.Add( PnDs );
        }
    }
}
