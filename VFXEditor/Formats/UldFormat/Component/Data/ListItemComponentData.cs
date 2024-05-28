using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Data {
    public class ListItemComponentData : UldGenericData {
        public ListItemComponentData() {
            Parsed.AddRange( [
                new ParsedUInt( "未知节点 ID 1" ),
                new ParsedUInt( "未知节点 ID 2" ),
                new ParsedUInt( "未知节点 ID 3" ),
                new ParsedUInt( "未知节点 ID 4" ),
            ] );
        }
    }
}
