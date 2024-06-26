﻿using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Node.Data.Component {
    public class RadioButtonNodeData : UldNodeComponentData {
        public RadioButtonNodeData() : base() {
            Parsed.AddRange( [
                new ParsedUInt( "文本 ID" ),
                new ParsedUInt( "组 ID" ),
            ] );
        }
    }
}
