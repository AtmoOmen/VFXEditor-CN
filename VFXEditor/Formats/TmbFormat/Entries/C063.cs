using Dalamud.Interface;
using System.Collections.Generic;
using VfxEditor.Parsing;
using VfxEditor.TmbFormat.Utils;

namespace VfxEditor.TmbFormat.Entries {
    public class C063 : TmbEntry {
        public const string MAGIC = "C063";
        public const string DISPLAY_NAME = "音效";
        public override string DisplayName => DISPLAY_NAME;
        public override string Magic => MAGIC;

        public override int Size => 0x20;
        public override int ExtraSize => 0;

        private readonly ParsedInt Loop = new( "循环/持续时间", value: 1 );
        private readonly ParsedInt Interrupt = new( "打断" );
        private readonly TmbOffsetString Path = new( "路径", null, true );
        private readonly ParsedInt SoundIndex = new( "音频索引" );
        private readonly ParsedByte SoundPosition = new( "音频位置", value: 1 );
        private readonly ParsedByte BindId = new( "绑定 Id" );
        private readonly ParsedShort Unknown1 = new( "未知 1" );

        public C063( TmbFile file ) : base( file ) {
            SetupIcon();
        }

        public C063( TmbFile file, TmbReader reader ) : base( file, reader ) {
            SetupIcon();
        }

        private void SetupIcon() {
            Path.Icons.Insert( 0, new() {
                Icon = () => FontAwesomeIcon.VolumeUp,
                Remove = false,
                Action = ( string value ) => Plugin.ResourceLoader.PlaySound( value, SoundIndex.Value )
            } );
        }

        protected override List<ParsedBase> GetParsed() => [
            Loop,
            Interrupt,
            Path,
            SoundIndex,
            SoundPosition,
            BindId,
            Unknown1
        ];
    }
}
