using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Data {
    public class JournalComponentData : UldGenericData {
        public JournalComponentData() {
            AddUnknown( 38, "未知节点 ID" );

            Parsed.AddRange( [
                new ParsedUInt( "项边距", size: 2 ),
                new ParsedUInt( "基本边距", size: 2 ),
                new ParsedUInt( "未知边距", size: 2 ),
                new ParsedReserve( 2 ) // Padding
            ] );
        }
    }
}
