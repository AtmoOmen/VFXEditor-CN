using System.Collections.Generic;
using VfxEditor.Parsing;
using VfxEditor.TmbFormat.Utils;
using VfxEditor.Utils;

namespace VfxEditor.TmbFormat.Entries {
    public class C168 : TmbEntry {
        public const string MAGIC = "C168";
        public const string DISPLAY_NAME = "强制摄像机控制";
        public override string DisplayName => DISPLAY_NAME;
        public override string Magic => MAGIC;
        public override DangerLevel Danger => DangerLevel.Yellow;

        public override int Size => 0x38;
        public override int ExtraSize => 0;

        private readonly ParsedInt Duration = new( "持续时间" );
        private readonly ParsedInt Unk2 = new( "未知 2" );
        private readonly ParsedInt TmfcId = new( "F 曲线 ID" );
        private readonly ParsedByte Unk4 = new( "未知 4" );
        private readonly ParsedByte Unk5 = new( "未知 5" );
        private readonly ParsedShort Unk6 = new( "未知 6" );
        private readonly ParsedInt Unk7 = new( "未知 7" );
        private readonly ParsedInt Unk8 = new( "未知 8" );
        private readonly ParsedInt Unk9 = new( "未知 9" );
        private readonly ParsedInt Unk10 = new( "未知 10" );
        private readonly ParsedInt Unk11 = new( "未知 11" );
        private readonly ParsedInt Unk12 = new( "未知 12" );
        private readonly ParsedInt Unk13 = new( "未知 13" );

        public C168( TmbFile file ) : base( file ) { }

        public C168( TmbFile file, TmbReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => [
            Duration,
            Unk2,
            TmfcId,
            Unk4,
            Unk5,
            Unk6,
            Unk7,
            Unk8,
            Unk9,
            Unk10,
            Unk11,
            Unk12,
            Unk13
        ];
    }
}
