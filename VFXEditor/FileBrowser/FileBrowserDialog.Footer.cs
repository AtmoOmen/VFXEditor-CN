using Dalamud.Interface.Utility.Raii;
using ImGuiNET;
using VfxEditor.Utils;

namespace VfxEditor.FileBrowser {
    public partial class FileBrowserDialog {
        private bool DrawFooter() {
            ImGui.SetCursorPosY( ImGui.GetCursorPosY() - ImGui.GetStyle().ItemSpacing.Y );
            ImGui.Separator();

            ImGui.SetNextItemWidth( ImGui.GetContentRegionAvail().X - 100 - ( Filters.Filters.Count == 0 ? 0 : 200 ) );
            using( var disabled = ImRaii.Disabled( SelectOnly ) ) {
                ImGui.InputText( "##FileName", ref FileNameInput, 255, SelectOnly ? ImGuiInputTextFlags.ReadOnly : ImGuiInputTextFlags.None );
            }

            Filters.Draw();

            var res = false;

            using( var disabled = ImRaii.Disabled( string.IsNullOrEmpty( FileNameInput ) || ( SelectOnly && Selected == null ) ) ) {
                ImGui.SameLine();
                if( ImGui.Button( "确认" ) ) {
                    IsOk = true;
                    res = true;
                }
            }

            using var style = ImRaii.PushStyle( ImGuiStyleVar.ItemSpacing, ImGui.GetStyle().ItemInnerSpacing );
            ImGui.SameLine();
            if( UiUtils.RemoveButton( "取消" ) ) {
                IsOk = false;
                res = true;
            }

            if( WantsToQuit && IsOk ) return true;

            return res;
        }
    }
}
