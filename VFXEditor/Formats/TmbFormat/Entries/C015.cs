using System.Collections.Generic;
using VfxEditor.Parsing;
using VfxEditor.TmbFormat.Utils;

namespace VfxEditor.TmbFormat.Entries {
    public enum AtchState {
        State_1 = 0,
        State_0 = 1,
        State_2 = 2,
        State_3 = 3,
        State_4 = 4,
        State_5 = 5,
    }
    public class C015 : TmbEntry {
        public const string MAGIC = "C015";
        public const string DISPLAY_NAME = "武器大小";
        public override string DisplayName => DISPLAY_NAME;
        public override string Magic => MAGIC;

        public override int Size => 0x1C;
        public override int ExtraSize => 0;

        private readonly ParsedInt Duration = new( "持续时间" );
        private readonly ParsedInt Unk2 = new( "未知 2" );
        private readonly ParsedEnum<AtchState> WeaponSize = new( "ATCH 物体缩放" );
        private readonly ParsedEnum<ObjectControl> ObjectControl = new( "物体控制" );

        public C015( TmbFile file ) : base( file ) { }

        public C015( TmbFile file, TmbReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => [
            Duration,
            Unk2,
            WeaponSize,
            ObjectControl
        ];
    }
}
