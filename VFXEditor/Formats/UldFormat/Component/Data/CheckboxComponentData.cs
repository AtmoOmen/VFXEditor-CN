using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Data {
    public class CheckboxComponentData : UldGenericData {
        public CheckboxComponentData() {
            Parsed.AddRange( [
                new ParsedUInt( "文本节点 ID" ),
                new ParsedUInt( "未知节点 ID 2" ),
                new ParsedUInt( "未知节点 ID 3" ),
            ] );
        }
    }
}
