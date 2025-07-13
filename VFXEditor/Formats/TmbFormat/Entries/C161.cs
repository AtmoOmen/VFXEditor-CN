using System.Collections.Generic;
using VfxEditor.Parsing;
using VfxEditor.TmbFormat.Utils;

namespace VfxEditor.TmbFormat.Entries {
    public class C161 : TmbEntry {
        public const string MAGIC = "C161";
        public const string DISPLAY_NAME = "闪烁";
        public override string DisplayName => DISPLAY_NAME;
        public override string Magic => MAGIC;

        public override int Size => 0x1C;
        public override int ExtraSize => 0;

        private readonly ParsedInt Enabled = new( "启用" );
        private readonly ParsedInt Unk2 = new( "未知 2" );
        private readonly ParsedBool Blink = new( "闪烁" );
        private readonly ParsedInt Unk4 = new( "未知 4" );

        public C161( TmbFile file ) : base( file ) { }

        public C161( TmbFile file, TmbReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => [
            Enabled,
            Unk2,
            Blink,
            Unk4
        ];
    }
}
