using ImGuiNET;
using System.Collections.Generic;

namespace VfxEditor.Select.Tabs.Npc {
    public class NpcTabTmb : NpcTab {
        public NpcTabTmb( SelectDialog dialog, string name ) : base( dialog, name ) { }

        protected override void DrawSelected() {
            ImGui.Text( "分支: " + Selected.Variant );
            DrawPaths( "时间线", Loaded, Selected.Name );
        }

        protected override void GetLoadedFiles( NpcFilesStruct files, out List<string> loaded ) {
            loaded = files.tmb;
        }
    }
}