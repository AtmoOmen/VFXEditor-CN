﻿using System.Collections.Generic;
using VfxEditor.Parsing;
using VfxEditor.TmbFormat.Utils;
using VfxEditor.Utils;

namespace VfxEditor.TmbFormat.Entries {
    public class C124 : TmbEntry {
        public const string MAGIC = "C124";
        public const string DISPLAY_NAME = "";
        public override string DisplayName => DISPLAY_NAME;
        public override string Magic => MAGIC;
        public override DangerLevel Danger => DangerLevel.Red;

        public override int Size => 0x18;
        public override int ExtraSize => 0;

        private readonly ParsedInt Unk1 = new( "未知 1", value: 1 );
        private readonly ParsedInt Unk2 = new( "未知 2" );
        private readonly ParsedInt Unk3 = new( "未知 3", value: 100 );

        public C124( TmbFile file ) : base( file ) { }

        public C124( TmbFile file, TmbReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => [
            Unk1,
            Unk2,
            Unk3
        ];
    }
}
