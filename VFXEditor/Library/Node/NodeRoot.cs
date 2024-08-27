using Dalamud.Interface.Utility.Raii;
using ImGuiNET;
using System.Collections.Generic;
using VfxEditor.Library.Components;

namespace VfxEditor.Library.Node {
    public class NodeRoot : LibraryRoot {
        public NodeRoot( List<LibraryProps> items ) : base( "Nodes", items ) { }

        public override bool Draw( LibraryManager library, string searchInput ) {
            using var child = ImRaii.Child( "子级", ImGui.GetContentRegionAvail(), false );

            if( Children.Count == 0 ) ImGui.TextDisabled( "未保存任何节点..." );
            return base.Draw( library, searchInput );
        }
    }
}
