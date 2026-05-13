using Dalamud.Interface.Utility.Raii;
using Dalamud.Bindings.ImGui;
using System.Collections.Generic;
using System.Linq;
using VfxEditor.FileManager;
using VfxEditor.Select.Formats;
using VfxEditor.Utils;
using VfxEditor.Formats.AvfxFormat;

namespace VfxEditor.AvfxFormat {
    public class AvfxManager : FileManager<AvfxDocument, AvfxFile, WorkspaceMetaRenamed> {
        public AvfxManager( AvfxManagerGroup group ) : base( group ) {
            SourceSelect = new VfxSelectDialog( "File Select [LOADED]", this, true );
            ReplaceSelect = new VfxSelectDialog( "File Select [REPLACED]", this, false );
        }

        protected override AvfxDocument GetNewDocument() => new( this, NewWriteLocation );

        protected override AvfxDocument GetWorkspaceDocument( WorkspaceMetaRenamed data, string localPath ) => new( this, NewWriteLocation, localPath, data );

        protected override void DrawEditMenuItems() {
            if( ImGui.BeginMenu( "模板" ) ) {
                using var disabledTemplates = ImRaii.Disabled( ActiveDocument == null );
                if( ImGui.MenuItem( "空白" ) ) ActiveDocument?.OpenTemplate( "default_vfx.avfx" );
                if( ImGui.MenuItem( "武器" ) ) ActiveDocument?.OpenTemplate( "default_weapon.avfx" );
                ImGui.EndMenu();
            }

            if( ImGui.BeginMenu( "转换纹理" ) ) {
                using var style = ImRaii.PushStyle( ImGuiStyleVar.ItemSpacing, ImGui.GetStyle().ItemInnerSpacing );

                ImGui.SetNextItemWidth( 150 );
                if( ImGui.InputText( "##Prefix", ref Plugin.Configuration.CustomPathPrefix, 255 ) ) Plugin.Configuration.Save();

                ImGui.SameLine();
                if( ImGui.Button( "应用" ) ) {
                    foreach( var file in Documents.Where( x => x.File != null ).Select( x => x.File ) ) {
                        var commands = new List<ICommand>();
                        file.TextureView.Group.Items.ForEach( x => x.ConvertToCustom( commands ) );
                        file.Command.AddAndExecute( new CompoundCommand( commands ) );
                    }
                }
                ImGui.EndMenu();
            }

            using var disabled = ImRaii.Disabled( ActiveFile == null );
            if( ImGui.MenuItem( "清理" ) ) ActiveFile?.Cleanup();
        }

        public void Import( string path ) => ActiveDocument?.Import( path );
    }
}
