using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Data {
    public class SliderComponentData : UldGenericData {
        public SliderComponentData() {
            Parsed.AddRange( [
                new ParsedUInt( "九宫格节点 ID" ),
                new ParsedUInt( "未知节点 ID 2" ),
                new ParsedUInt( "文本节点 ID" ),
                new ParsedUInt( "未知节点 ID 4" ),
                new ParsedByteBool( "是否垂直" ),
                new ParsedUInt( "左部偏移", size: 1 ),
                new ParsedUInt( "右部偏移", size: 1),
                new ParsedInt( "内边距", size: 1)
            ] );
        }
    }
}
