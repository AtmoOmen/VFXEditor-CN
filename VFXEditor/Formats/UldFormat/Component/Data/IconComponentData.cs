﻿using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Data {
    public class IconComponentData : UldGenericData {
        public IconComponentData() {
            Parsed.AddRange( [
                new ParsedUInt( "未知节点 ID 1" ),
                new ParsedUInt( "未知节点 ID 2" ),
                new ParsedUInt( "未知节点 ID 3" ),
                new ParsedUInt( "未知节点 ID 4" ),
                new ParsedUInt( "未知节点 ID 5" ),
                new ParsedUInt( "未知节点 ID 6" ),
                new ParsedUInt( "未知节点 ID 7" ),
                new ParsedUInt( "未知节点 ID 8" ),
            ] );
        }
    }
}
