﻿using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Node.Data.Component {
    public class CheckboxNodeData : UldNodeComponentData {
        public CheckboxNodeData() : base() {
            Parsed.AddRange( [
                new ParsedUInt( "文本 ID" )
            ] );
        }
    }
}
