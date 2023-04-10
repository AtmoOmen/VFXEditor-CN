using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Data {
    public class GaugeComponentData : UldComponentData {
        public GaugeComponentData() {
            Parsed.AddRange( new ParsedBase[] {
                new ParsedUInt( "Unknown 1" ),
                new ParsedUInt( "Unknown 2" ),
                new ParsedUInt( "Unknown 3" ),
                new ParsedUInt( "Unknown 4" ),
                new ParsedUInt( "Unknown 5" ),
                new ParsedUInt( "Unknown 6" ),
                new ParsedUInt( "Vertical Margin", size: 2 ),
                new ParsedUInt( "Horizontal Margin", size: 2 ),
                new ParsedBool( "Is Vertical", size: 1 ),
                new ParsedReserve( 3 ) // Padding
            } );
        }
    }
}