using System.Collections.Generic;
using VfxEditor.Parsing;
using VfxEditor.TmbFormat.Utils;
using VfxEditor.Utils;

namespace VfxEditor.TmbFormat.Entries {
    public class C176 : TmbEntry {
        public const string MAGIC = "C176";
        public const string DISPLAY_NAME = "强制垂直运动";
        public override string DisplayName => DISPLAY_NAME;
        public override string Magic => MAGIC;
        public override DangerLevel Danger => DangerLevel.Detectable;

        public override int Size => 0x28;
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

        public C176( TmbFile file ) : base( file ) { }

        public C176( TmbFile file, TmbReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => [
            Duration,
            Unk2,
            TmfcId,
            Unk4,
            Unk5,
            Unk6,
            Unk7,
            Unk8,
            Unk9
        ];
    }
}
