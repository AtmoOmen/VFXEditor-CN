using VfxEditor.Parsing;
using VfxEditor.Parsing.Color;

namespace VfxEditor.UldFormat.Component.Data {
    public class NumericInputComponentData : UldGenericData {
        public NumericInputComponentData() {
            Parsed.AddRange( [
                new ParsedUInt( "未知节点 ID 1" ),
                new ParsedUInt( "未知节点 ID 2" ),
                new ParsedUInt( "未知节点 ID 3" ),
                new ParsedUInt( "未知节点 ID 4" ),
                new ParsedUInt( "未知节点 ID 5" ),
                new ParsedSheetColor( "颜色" ),
            ] );
        }
    }
}
