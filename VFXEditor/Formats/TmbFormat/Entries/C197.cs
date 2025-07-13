using System.Collections.Generic;
using VfxEditor.Parsing;
using VfxEditor.TmbFormat.Utils;

namespace VfxEditor.TmbFormat.Entries {
    public class C197 : TmbEntry {
        public enum SpeakTmbType {
            Normal = 0,
            Whisper = 1,
            Shout = 2,
            Disabled = 3,
        }
        public const string MAGIC = "C197";
        public const string DISPLAY_NAME = "语音台词";
        public override string DisplayName => DISPLAY_NAME;
        public override string Magic => MAGIC;

        public override int Size => 0x24;
        public override int ExtraSize => 0;

        private readonly ParsedInt FadeTime = new( "淡化时间" );
        private readonly ParsedInt Unk2 = new( "未知 2" );
        private readonly ParsedInt VoicelineNumber = new( "语音台词编号" );
        private readonly ParsedInt BindPointId = new( "绑定点 ID" );
        private readonly ParsedEnum<SpeakTmbType> SpeakType = new( "语音类型" );
        private readonly ParsedInt Unk6 = new( "未知 6" );

        public C197( TmbFile file ) : base( file ) { }

        public C197( TmbFile file, TmbReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => [
            FadeTime,
            Unk2,
            VoicelineNumber,
            BindPointId,
            SpeakType,
            Unk6
        ];
    }
}
