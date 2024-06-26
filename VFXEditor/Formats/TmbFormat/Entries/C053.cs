﻿using System;
using System.Collections.Generic;
using VfxEditor.Parsing;
using VfxEditor.TmbFormat.Utils;

namespace VfxEditor.TmbFormat.Entries {
    [Flags]
    public enum SoundFlags {
        Stop_on_Movement = 0x01,
        Overlap_Sounds = 0x02,
        Unknown_1 = 0x04,
        Unknown_2 = 0x08,
        Unknown_3 = 0x10,
        Unknown_4 = 0x20,
        Unknown_5 = 0x40,
        Unknown_6 = 0x80,
    }

    public class C053 : TmbEntry {
        public const string MAGIC = "C053";
        public const string DISPLAY_NAME = "语音台词";
        public override string DisplayName => DISPLAY_NAME;
        public override string Magic => MAGIC;

        public override int Size => 0x1C;
        public override int ExtraSize => 0;

        private readonly ParsedInt Unk1 = new( "未知 1" );
        private readonly ParsedInt Unk2 = new( "未知 2" );
        private readonly ParsedShort SoundId1 = new( "音频 ID 1" );
        private readonly ParsedShort SoundId2 = new( "音频 ID 2" );
        private readonly ParsedShort Unk3 = new( "未知 3" );
        private readonly ParsedFlag<SoundFlags> Flags = new( "标识", size: 2 );

        public C053( TmbFile file ) : base( file ) { }

        public C053( TmbFile file, TmbReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => [
            Unk1,
            Unk2,
            SoundId1,
            SoundId2,
            Unk3,
            Flags,
        ];
    }
}
