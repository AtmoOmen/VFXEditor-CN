using System.Collections.Generic;
using VfxEditor.Parsing;
using VfxEditor.TmbFormat.Utils;

namespace VfxEditor.TmbFormat.Entries {
    public class C144 : TmbEntry {
        public const string MAGIC = "C144";
        public const string DISPLAY_NAME = "摄像机和铭牌控制";
        public override string DisplayName => DISPLAY_NAME;
        public override string Magic => MAGIC;

        public override int Size => 0x2C;
        public override int ExtraSize => 0;

        private readonly ParsedInt Duration = new( "持续时间" );
        private readonly ParsedInt Unk2 = new( "未知 2" );
        private readonly ParsedInt Unk3 = new( "未知 3" );
        private readonly ParsedFloat2 Camera = new( "摄像机转变" );
        private readonly ParsedFloat3 Nameplate = new( "铭牌转变" );

        public C144( TmbFile file ) : base( file ) { }

        public C144( TmbFile file, TmbReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => [
            Duration,
            Unk2,
            Unk3,
            Camera,
            Nameplate
        ];
    }
}
