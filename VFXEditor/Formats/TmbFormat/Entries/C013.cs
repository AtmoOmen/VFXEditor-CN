using System.Collections.Generic;
using VfxEditor.Parsing;
using VfxEditor.TmbFormat.Utils;
using VfxEditor.Utils;

namespace VfxEditor.TmbFormat.Entries {
    public class C013 : TmbEntry {
        public const string MAGIC = "C013";
        public const string DISPLAY_NAME = "";
        public override string DisplayName => DISPLAY_NAME;
        public override string Magic => MAGIC;

        public override int Size => 0x1C;
        public override int ExtraSize => 0;
        public override DangerLevel Danger => DangerLevel.Yellow;

        private readonly ParsedInt Duration = new( "持续时间" );
        private readonly ParsedInt Unk2 = new( "未知 2" );
        private readonly ParsedInt TmfcId = new( "F 曲线 ID" );
        private readonly ParsedInt Unk4 = new( "未知 4" );

        public C013( TmbFile file ) : base( file ) { }

        public C013( TmbFile file, TmbReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => [
            Duration,
            Unk2,
            TmfcId,
            Unk4
        ];
    }
}
