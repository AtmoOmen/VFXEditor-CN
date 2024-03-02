using Dalamud.Interface.Utility.Raii;
using ImGuiNET;
using System.Collections.Generic;
using VfxEditor.FileBrowser;
using VfxEditor.Utils;

namespace VfxEditor.AvfxFormat.Dialogs {
    public class AvfxExport {
        private readonly List<AvfxExportCategory> Categories;
        private bool ExportDependencies = true;

        public AvfxExport( AvfxFile file ) {
            Categories = new() {
                new ExportDialogCategory<AvfxTimeline>( file.NodeGroupSet.Timelines, "时间线" ),
                new ExportDialogCategory<AvfxEmitter>( file.NodeGroupSet.Emitters, "发射器" ),
                new ExportDialogCategory<AvfxParticle>( file.NodeGroupSet.Particles, "粒子" ),
                new ExportDialogCategory<AvfxEffector>( file.NodeGroupSet.Effectors, "效果器" ),
                new ExportDialogCategory<AvfxBinder>( file.NodeGroupSet.Binders, "绑定器" ),
                new ExportDialogCategory<AvfxTexture>( file.NodeGroupSet.Textures, "材质" ),
                new ExportDialogCategory<AvfxModel>( file.NodeGroupSet.Models, "模型" )
            };
        }

        public void Reset() => Categories.ForEach( cat => cat.Reset() );

        public void Draw() {
            using var _ = ImRaii.PushId( "##ExportDialog" );

            ImGui.Checkbox( "导出依赖关系", ref ExportDependencies );

            ImGui.SameLine();
            UiUtils.HelpMarker( @"导出选中项与它们的依赖关系 (如: 依赖材质的例子效果)。推荐选中此项" );

            ImGui.SameLine();
            if( ImGui.Button( "重置" ) ) Reset();

            ImGui.SameLine();
            if( ImGui.Button( "导出" ) ) SaveDialog();

            using var child = ImRaii.Child( "子级", new( -1, -1 ), true );
            Categories.ForEach( cat => cat.Draw() );
        }

        public void Show( AvfxNode node ) {
            Plugin.AvfxManager?.ExportDialog.Show();
            Reset();
            foreach( var category in Categories ) {
                if( category.Belongs( node ) ) {
                    category.Select( node );
                    break;
                }
            }
        }

        public List<AvfxNode> GetSelected() {
            var result = new List<AvfxNode>();
            Categories.ForEach( x => result.AddRange( x.GetSelectedNodes() ) );
            return result;
        }

        public void SaveDialog() {
            FileBrowserManager.SaveFileDialog( "选择保存位置", ".vfxedit2,.*", "ExportedVfx", "vfxedit2", ( bool ok, string res ) => {
                if( !ok ) return;
                AvfxFile.Export( GetSelected(), res, ExportDependencies );
                Plugin.AvfxManager?.ExportDialog.Hide();
            } );
        }
    }
}
