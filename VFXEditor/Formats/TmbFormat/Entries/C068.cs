﻿using System.Collections.Generic;
using VfxEditor.Parsing;
using VfxEditor.TmbFormat.Utils;

namespace VfxEditor.TmbFormat.Entries {
    public class C068 : TmbEntry {
        public const string MAGIC = "C068";
        public const string DISPLAY_NAME = "Shade Color";
        public override string DisplayName => DISPLAY_NAME;
        public override string Magic => MAGIC;

        public override int Size => 0x24;
        public override int ExtraSize => 4 * ( 4 + 4 );

        private readonly ParsedInt Unk1 = new( "未知 1" );
        private readonly ParsedInt Unk2 = new( "未知 2" );
        private readonly TmbOffsetFloat4 Color1 = new( "颜色 1", defaultValue: new( 1 ) );
        private readonly TmbOffsetFloat4 Color2 = new( "颜色 2", defaultValue: new( 1 ) );

        public C068( TmbFile file ) : base( file ) { }

        public C068( TmbFile file, TmbReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => [
            Unk1,
            Unk2,
            Color1,
            Color2
        ];
    }
}
