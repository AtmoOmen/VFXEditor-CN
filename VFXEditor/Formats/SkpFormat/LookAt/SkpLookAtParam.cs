using System.Collections.Generic;
using System.IO;
using System.Linq;
using VfxEditor.Parsing;
using VfxEditor.Ui.Interfaces;
using VfxEditor.Utils;

namespace VfxEditor.Formats.SkpFormat.LookAt {
    public class SkpLookAtParam : ParsedData, IUiItem, ITextItem {
        public readonly ParsedByte Index = new( "索引" );

        public SkpLookAtParam() : base() { }

        public SkpLookAtParam( BinaryReader reader ) : base() {
            ReadParsed( reader );
            reader.ReadBytes( 3 );
        }

        protected override List<ParsedBase> GetParsed() => new() {
            new ParsedFloat4( "角度限制" ),
            new ParsedFloat3( "前向旋转" ),
            new ParsedFloat( "角度限制" ),
            new ParsedFloat3( "眼部位置" ),
            new ParsedUInt( "标识" ),
            new ParsedFloat( "增益" ),
            Index,
        };

        public void Write( BinaryWriter writer ) {
            WriteParsed( writer );
            FileUtils.Pad( writer, 3 );
        }

        public void Draw() {
            // Draw the index first
            Index.Draw();
            foreach( var item in Parsed.Where( x => x != Index ) ) item.Draw();
        }

        public string GetText() => $"参数 {Index.Value}";
    }
}
