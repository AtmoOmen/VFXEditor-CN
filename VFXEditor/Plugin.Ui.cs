using ImGuiNET;
using OtterGui.Raii;
using System;
using System.Collections.Generic;
using System.Linq;
using VfxEditor.Data;
using VfxEditor.FileManager;
using VfxEditor.TextureFormat;
using VfxEditor.Ui.Components;
using VfxEditor.Ui.Export;
using VfxEditor.Utils;

namespace VfxEditor {
    public partial class Plugin {
        public static readonly Dictionary<string, Modal> Modals = new();

        public static void Draw() {
            if( Loading ) return;

            CopyManager.ResetAll();
            CheckWorkspaceKeybinds();

            TexToolsDialog.Draw();
            PenumbraDialog.Draw();
            ToolsDialog.Draw();
            Tracker.Draw();
            Configuration.Draw();
            LibraryManager.Draw();

            Managers.ForEach( x => x?.Draw() );

            CopyManager.FinalizeAll();

            if( Configuration.AutosaveEnabled &&
                Configuration.AutosaveSeconds > 10 &&
                !string.IsNullOrEmpty( CurrentWorkspaceLocation ) &&
                ( DateTime.Now - LastAutoSave ).TotalSeconds > Configuration.AutosaveSeconds
            ) {
                LastAutoSave = DateTime.Now;
                SaveWorkspace();
            }

            foreach( var modal in Modals ) modal.Value.Draw();
        }

        private static void CheckWorkspaceKeybinds() {
            if( Configuration.OpenKeybind.KeyPressed() ) OpenWorkspace();
            if( Configuration.SaveKeybind.KeyPressed() ) SaveWorkspace();
            if( Configuration.SaveAsKeybind.KeyPressed() ) SaveAsWorkspace();
        }

        public static void DrawFileMenu() {
            using var _ = ImRaii.PushId( "Menu" );

            if( ImGui.BeginMenu( "File" ) ) {
                ImGui.TextDisabled( "Workspace" );
                ImGui.SameLine();
                UiUtils.HelpMarker( "A workspace allows you to save multiple vfx replacements at the same time, as well as any imported textures or item renaming (such as particles or emitters)" );

                if( ImGui.MenuItem( "New" ) ) NewWorkspace();
                if( ImGui.MenuItem( "Open" ) ) OpenWorkspace();
                if( ImGui.MenuItem( "Save" ) ) SaveWorkspace();
                if( ImGui.MenuItem( "Save As" ) ) SaveAsWorkspace();

                ImGui.Separator();
                if( ImGui.MenuItem( "Settings" ) ) Configuration.Show();
                if( ImGui.MenuItem( "Tools" ) ) ToolsDialog.Show();
                if( ImGui.BeginMenu( "Help" ) ) {
                    if( ImGui.MenuItem( "Github" ) ) UiUtils.OpenUrl( "https://github.com/0ceal0t/Dalamud-VFXEditor" );
                    if( ImGui.MenuItem( "Report an Issue" ) ) UiUtils.OpenUrl( "https://github.com/0ceal0t/Dalamud-VFXEditor/issues" );
                    if( ImGui.MenuItem( "Wiki" ) ) UiUtils.OpenUrl( "https://github.com/0ceal0t/Dalamud-VFXEditor/wiki" );
                    ImGui.EndMenu();
                }

                ImGui.EndMenu();
            }

            if( ImGui.BeginMenu( "Export" ) ) {
                if( ImGui.MenuItem( "Penumbra" ) ) PenumbraDialog.Show();
                if( ImGui.MenuItem( "TexTools" ) ) TexToolsDialog.Show();
                ImGui.EndMenu();
            }
        }

        public static void DrawManagersMenu( IFileManager currentManager ) {
            using var _ = ImRaii.PushId( "Menu" );

            if( ImGui.MenuItem( "Textures" ) ) TextureManager.Show();
            ImGui.Separator();
            Managers.Where( x => x != TextureManager ).ToList().ForEach( x => DrawManagerMenu( x, currentManager ) );
        }

        private static void DrawManagerMenu( IFileManager manager, IFileManager currentManager ) {
            using var disabled = ImRaii.Disabled( manager == currentManager );
            if( ImGui.MenuItem( manager.GetExportName() ) ) manager.Show();
        }

        public static void AddModal( Modal modal ) {
            Modals[modal.Title] = modal;
            ImGui.OpenPopup( modal.Title );
        }
    }
}