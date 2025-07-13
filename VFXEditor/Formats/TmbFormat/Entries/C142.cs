using System.Collections.Generic;
using VfxEditor.Parsing;
using VfxEditor.TmbFormat.Utils;

namespace VfxEditor.TmbFormat.Entries {
    public enum FreezePositionType {
        Target_FixedDirection,
        Target_UserFacingDirection,
        Return
    }

    public class C142 : TmbEntry {
        public const string MAGIC = "C142";
        public const string DISPLAY_NAME = "冻结位置";
        public override string DisplayName => DISPLAY_NAME;
        public override string Magic => MAGIC;

        public override int Size => 0x1C;
        public override int ExtraSize => 0;

        private readonly ParsedInt Duration = new( "持续时间" );
        private readonly ParsedInt Unk2 = new( "未知 2" );
        private readonly ParsedInt Position = new( "绑定点" );
        private readonly ParsedEnum<FreezePositionType> FreezeLocation = new( "冻结位置" );

        public C142( TmbFile file ) : base( file ) { }

        public C142( TmbFile file, TmbReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => [
            Duration,
            Unk2,
            Position,
            FreezeLocation
        ];
    }
}
