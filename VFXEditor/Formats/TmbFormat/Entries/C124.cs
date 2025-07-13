using System.Collections.Generic;
using VfxEditor.Parsing;
using VfxEditor.TmbFormat.Utils;
using VfxEditor.Utils;

namespace VfxEditor.TmbFormat.Entries {
    public class C124 : TmbEntry {
        public const string MAGIC = "C124";
        public const string DISPLAY_NAME = "可选中";
        public override string DisplayName => DISPLAY_NAME;
        public override string Magic => MAGIC;
        public override DangerLevel Danger => DangerLevel.Red;

        public override int Size => 0x18;
        public override int ExtraSize => 0;

        private readonly ParsedBool Enabled = new( "启用" );
        private readonly ParsedInt Unk2 = new( "未知 2" );
        private readonly ParsedBool Targetable = new( "可选中" );

        public C124( TmbFile file ) : base( file ) { }

        public C124( TmbFile file, TmbReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => [
            Enabled,
            Unk2,
            Targetable
        ];
    }
}
