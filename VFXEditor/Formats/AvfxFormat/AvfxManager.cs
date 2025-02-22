using Dalamud.Interface.Utility.Raii;
using ImGuiNET;
using System.Collections.Generic;
using System.Linq;
using VfxEditor.FileManager;
using VfxEditor.Formats.AvfxFormat.Dialogs;
using VfxEditor.Select.Formats;
using VfxEditor.Utils;

namespace VfxEditor.AvfxFormat {
    public class AvfxManager : FileManager<AvfxDocument, AvfxFile, WorkspaceMetaRenamed> {
        public readonly AvfxExportDialog ExportDialog;

        public AvfxManager() : base( "VFXEditor", "Vfx", "avfx", "Docs", "VFX" ) {
            SourceSelect = new VfxSelectDialog( "选择文件 [原始]##AVFX", this, true );
            ReplaceSelect = new VfxSelectDialog( "选择文件 [替换]##AVFX", this, false );
            ExportDialog = new( WindowSystem );
        }

        protected override AvfxDocument GetNewDocument() => new( this, NewWriteLocation );

        protected override AvfxDocument GetWorkspaceDocument( WorkspaceMetaRenamed data, string localPath ) => new( this, NewWriteLocation, localPath, data );

        protected override void DrawEditMenuItems() {
            if( ImGui.BeginMenu( "模板" ) ) {
                if( ImGui.MenuItem( "空白" ) ) ActiveDocument?.OpenTemplate( "default_vfx.avfx" );
                if( ImGui.MenuItem( "武器" ) ) ActiveDocument?.OpenTemplate( "default_weapon.avfx" );
                ImGui.EndMenu();
            }

            if( ImGui.BeginMenu( "Convert Textures" ) ) {
                using var style = ImRaii.PushStyle( ImGuiStyleVar.ItemSpacing, ImGui.GetStyle().ItemInnerSpacing );

                ImGui.SetNextItemWidth( 150 );
                if( ImGui.InputText( "##Prefix", ref Plugin.Configuration.CustomPathPrefix, 255 ) ) Plugin.Configuration.Save();

                ImGui.SameLine();
                if( ImGui.Button( "Apply" ) ) {
                    foreach( var file in Documents.Where( x => x.File != null ).Select( x => x.File ) ) {
                        var commands = new List<ICommand>();
                        file.TextureView.Group.Items.ForEach( x => x.ConvertToCustom( commands ) );
                        file.Command.AddAndExecute( new CompoundCommand( commands ) );
                    }
                }
                ImGui.EndMenu();
            }

            using var disabled = ImRaii.Disabled( File == null );
            if( ImGui.MenuItem( "Clean Up" ) ) File?.Cleanup();
        }

        public void Import( string path ) => ActiveDocument.Import( path );

        public void ShowExportDialog( AvfxNode node ) => ActiveDocument.ShowExportDialog( node );
    }
}
