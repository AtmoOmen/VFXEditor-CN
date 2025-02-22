﻿using Dalamud.Interface.Utility.Raii;
using ImGuiNET;
using VfxEditor.FileManager.Interfaces;

namespace VfxEditor.Ui.Export {
    public abstract class ExportDialog : DalamudWindow {
        protected string ModName = "";
        protected string Author = "";
        protected string Version = "1.0.0";

        public ExportDialog( string id ) : base( id, false, new( 800, 600 ), Plugin.WindowSystem ) { }

        public override void DrawBody() {
            using var _ = ImRaii.PushId( WindowName );

            var width = ImGui.GetContentRegionAvail().X - ImGui.CalcTextSize( "导出" ).X - ImGui.GetStyle().ItemInnerSpacing.X - ImGui.GetStyle().FramePadding.X * 2;
            var inputWidth = width / 3.0f - ImGui.GetStyle().ItemInnerSpacing.X;

            using( var spacing = ImRaii.PushStyle( ImGuiStyleVar.ItemSpacing, ImGui.GetStyle().ItemInnerSpacing ) ) {
                ImGui.SetNextItemWidth( inputWidth );
                ImGui.InputTextWithHint( "##Name", "模组名", ref ModName, 255 );

                if( string.IsNullOrEmpty( Author ) ) Author = Plugin.Configuration.DefaultAuthor;

                ImGui.SameLine();
                ImGui.SetNextItemWidth( inputWidth );
                ImGui.InputTextWithHint( "##Author", "作者", ref Author, 255 );

                ImGui.SameLine();
                ImGui.SetNextItemWidth( inputWidth );
                ImGui.InputTextWithHint( "##Version", "版本", ref Version, 255 );

                ImGui.SameLine();
                if( ImGui.Button( "导出" ) ) OnExport();
            }

            ImGui.Separator();

            var footerHeight = ImGui.GetStyle().ItemSpacing.Y + ImGui.GetFrameHeightWithSpacing();

            using var child = ImRaii.Child( "Child" );
            OnDraw();
        }

        protected abstract void OnDraw();

        protected abstract void OnExport();

        protected abstract void OnReset();

        protected abstract void OnRemoveDocument( IFileDocument document );

        public static void Reset() {
            Plugin.PenumbraDialog.OnReset();
            Plugin.TexToolsDialog.OnReset();
        }

        public static void RemoveDocument( IFileDocument document ) {
            Plugin.PenumbraDialog.OnRemoveDocument( document );
            Plugin.TexToolsDialog.OnRemoveDocument( document );
        }
    }
}
