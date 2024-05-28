using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Data {
    public class DragDropComponentData : UldGenericData {
        public DragDropComponentData() {
            Parsed.AddRange( [
                new ParsedUInt( "未知节点 ID 1" ),
            ] );
        }
    }
}
