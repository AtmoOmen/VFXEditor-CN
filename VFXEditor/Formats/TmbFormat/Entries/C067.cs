﻿using System.Collections.Generic;
using VfxEditor.Parsing;
using VfxEditor.TmbFormat.Utils;

namespace VfxEditor.TmbFormat.Entries {
    public class C067 : TmbEntry {
        public const string MAGIC = "C067";
        public const string DISPLAY_NAME = "受击";
        public override string DisplayName => DISPLAY_NAME;
        public override string Magic => MAGIC;

        public override int Size => 0x14;
        public override int ExtraSize => 0;

        private readonly ParsedInt Enabled = new( "启用" );
        private readonly ParsedInt Unk2 = new( "未知 2" );

        public C067( TmbFile file ) : base( file ) { }

        public C067( TmbFile file, TmbReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => [
            Enabled,
            Unk2
        ];
    }
}
