﻿using ImGuiNET;

namespace VfxEditor.Select.Tabs.Emotes {
    public class EmoteTabVfx : EmoteTab<ParsedPaths> {
        public EmoteTabVfx( SelectDialog dialog, string name ) : base( dialog, name ) { }

        public override void LoadSelection( EmoteRow item, out ParsedPaths loaded ) => ParsedPaths.ReadFile( item.VfxPapFiles, SelectDataUtils.AvfxRegex, out loaded );

        protected override void DrawSelected() {
            DrawIcon( Selected.Icon );
            ImGui.TextDisabled( Selected.Command );

            DrawPaths( "视效", Loaded.Paths, Selected.Name );
        }
    }
}