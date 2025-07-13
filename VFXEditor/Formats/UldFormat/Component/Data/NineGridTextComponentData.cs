using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Data {
    public class NineGridTextComponentData : UldGenericData {
        public NineGridTextComponentData() {
            Parsed.AddRange( [
                new ParsedUInt( "文本节点 ID" ),
                new ParsedUInt( "未知节点 ID 2" ),
            ] );
        }
    }
}
