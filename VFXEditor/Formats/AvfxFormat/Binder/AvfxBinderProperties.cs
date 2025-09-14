using System.Collections.Generic;
using System.IO;
using VfxEditor.Formats.AvfxFormat.Binder;
using VfxEditor.Ui.Interfaces;
using VFXEditor.Formats.AvfxFormat.Curve;
using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxBinderProperties : AvfxOptional {
        public readonly string Name;

        public readonly AvfxEnum<BindPoint> BindPointType = new( "绑定点类型", "BPT" );
        public readonly AvfxEnum<BindTargetPoint> BindTargetPointType = new( "绑定目标点类型", "BPTP", value: BindTargetPoint.ByName );
        public readonly AvfxBinderPropertiesName BinderName = new();
        public readonly AvfxInt BindPointId = new( "绑定点 ID", "BPID", value: 3 );
        public readonly AvfxInt GenerateDelay = new( "生成延迟", "GenD" );
        public readonly AvfxInt CoordUpdateFrame = new( "坐标更新帧", "CoUF", value: -1 );
        public readonly AvfxBool RingEnable = new( "启用环", "bRng" );
        public readonly AvfxInt RingProgressTime = new( "环进度时间", "RnPT", value: 1 );
        public readonly AvfxFloat RingPositionX = new( "X 轴环位置", "RnPX" );
        public readonly AvfxFloat RingPositionY = new( "Y 轴环位置", "RnPY" );
        public readonly AvfxFloat RingPositionZ = new( "Z 轴环位置", "RnPZ" );
        public readonly AvfxFloat RingRadius = new( "环半径", "RnRd" );
        public readonly AvfxInt BCT = new( "BCT", "BCT" );
        public readonly AvfxCurve3Axis Position = new( "位置", "Pos", locked: true );

        private readonly List<AvfxBase> Parsed;

        private readonly UiDisplayList Parameters;
        private readonly List<INamedUiItem> DisplayTabs;

        private static readonly Dictionary< int, string > BinderIds = new()
        {
            { 0, "未工作" },
            { 1, "头部" },
            { 3, "左手武器" },
            { 4, "右手武器" },
            { 5, "武器尖端" },
            { 6, "右肩" },
            { 7, "左肩" },
            { 8, "右前臂" },
            { 9, "左前臂" },
            { 10, "右小腿" },
            { 11, "左小腿" },
            { 16, "角色前方" },
            { 25, "头顶" },
            { 26, "头部中央" },
            { 27, "额头" },
            { 28, "颈部" },
            { 29, "角色中心" },
            { 30, "角色中心" },
            { 31, "腰部" },
            { 32, "右手" },
            { 33, "左手" },
            { 34, "右脚" },
            { 35, "左脚" },
            { 42, "角色上方" },
            { 43, "头部 (偏右眼)" },
            { 44, "头部 (偏左眼)" },
            { 71, "角色原点" },
            { 77, "右手武器" },
            { 78, "左手武器" },
            { 107, "投掷点" },
            { 108, "贤者贤具3（前）/ 钐镰客颈部" },
            { 109, "贤者贤具3（后）/ 钐镰客脊椎" },
            { 110, "贤者贤具2（前）/ 钐镰客左手" },
            { 111, "贤者贤具2（后）/ 钐镰客面部" },
            { 112, "贤者贤具4（前）/ 钐镰客原点" },
            { 113, "贤者贤具4（后）" },
        };

        public AvfxBinderProperties( string name, string avfxName ) : base( avfxName ) {
            Name = name;

            Parsed = [
                BindPointType,
                BindTargetPointType,
                BinderName,
                BindPointId,
                GenerateDelay,
                CoordUpdateFrame,
                RingEnable,
                RingProgressTime,
                RingPositionX,
                RingPositionY,
                RingPositionZ,
                RingRadius,
                BCT,
                Position
            ];
            BinderName.SetAssigned( false );
            Position.SetAssigned( true );

            DisplayTabs = [
                ( Parameters = new UiDisplayList( "参数" ) ),
                Position
            ];

            Parameters.AddRange( [
                BindPointType,
                BindTargetPointType,
                BinderName,
                new UiIntCombo( "绑定点 ID", BindPointId, BinderIds ),
                GenerateDelay,
                CoordUpdateFrame,
                RingEnable,
                RingProgressTime,
                new UiFloat3( "环位置", RingPositionX, RingPositionY, RingPositionZ ),
                RingRadius,
                BCT
            ] );
        }

        public override void ReadContents( BinaryReader reader, int size ) => ReadNested( reader, Parsed, size );

        public override void WriteContents( BinaryWriter writer ) => WriteNested( writer, Parsed );

        protected override IEnumerable<AvfxBase> GetChildren() {
            foreach( var item in Parsed ) yield return item;
        }

        public override void DrawBody() {
            DrawNamedItems( DisplayTabs );
        }

        public override string GetDefaultText() => Name;
    }
}
