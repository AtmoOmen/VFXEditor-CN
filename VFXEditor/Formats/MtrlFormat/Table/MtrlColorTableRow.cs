using Dalamud.Interface.Utility.Raii;
using ImGuiNET;
using System.IO;
using System.Numerics;
using VfxEditor.Data.Command;
using VfxEditor.DirectX;
using VfxEditor.Formats.MtrlFormat.Stm;
using VfxEditor.Parsing.HalfFloat;
using VfxEditor.Ui.Interfaces;

namespace VfxEditor.Formats.MtrlFormat.Table {
    public class MtrlColorTableRow : IUiItem {
        public readonly int RenderId = Renderer.NewId;
        private readonly MtrlFile File;

        public MtrlDyeTableRow DyeRow { get; private set; }
        public StmDyeData DyeData { get; private set; }

        private MtrlDye PreviewDye;

        public const int Size = 32; // 16 ushorts

        public readonly ParsedHalf3Color Diffuse = new( "漫射", Vector3.One );
        public readonly ParsedHalf SpecularStrength = new( "镜面反射强度", 1f );
        public readonly ParsedHalf3Color Specular = new( "镜面", Vector3.One );
        public readonly ParsedHalf GlossStrength = new( "光泽强度", 20f );
        public readonly ParsedHalf3Color Emissive = new( "自发光" );
        public readonly ParsedTileMaterial TileMaterial = new( "Tile Material" );
        public readonly ParsedHalf MaterialRepeatX = new( "横轴材质重复", 16f );
        public readonly ParsedHalf2 MaterialSkew = new( "材质倾斜" );
        public readonly ParsedHalf MaterialRepeatY = new( "纵轴材质重复", 16f );

        public MtrlColorTableRow( MtrlFile file ) {
            File = file;
        }

        public MtrlColorTableRow( MtrlFile file, BinaryReader reader ) : this( file ) {
            Diffuse.Read( reader );
            SpecularStrength.Read( reader );
            Specular.Read( reader );
            GlossStrength.Read( reader );
            Emissive.Read( reader );
            TileMaterial.Read( reader );
            MaterialRepeatX.Read( reader );
            MaterialSkew.Read( reader );
            MaterialRepeatY.Read( reader );
        }

        public void Write( BinaryWriter writer ) {
            Diffuse.Write( writer );
            SpecularStrength.Write( writer );
            Specular.Write( writer );
            GlossStrength.Write( writer );
            Emissive.Write( writer );
            TileMaterial.Write( writer );
            MaterialRepeatX.Write( writer );
            MaterialSkew.Write( writer );
            MaterialRepeatY.Write( writer );
        }

        public void SetDyeRow( MtrlDyeTableRow dye ) { DyeRow = dye; }

        public void SetPreviewDye( MtrlDye previewDye ) {
            PreviewDye = previewDye;
            RefreshDye();
        }

        private void DrawColor() {
            Diffuse.Draw();
            SpecularStrength.Draw();
            Specular.Draw();
            GlossStrength.Draw();
            Emissive.Draw();
            TileMaterial.Draw();
            MaterialRepeatX.Draw();
            MaterialRepeatY.Draw();
            MaterialSkew.Draw();
        }

        private void RefreshDye() {
            DyeData = PreviewDye == null ? null : Plugin.MtrlManager.Stm.GetDye( DyeRow.Template.Value, ( int )PreviewDye.Id );
        }

        public void Draw() {
            using var editing = new Edited();

            using var tabBar = ImRaii.TabBar( "栏", ImGuiTabBarFlags.NoCloseWithMiddleMouseButton );
            if( !tabBar ) return;

            using( var tab = ImRaii.TabItem( "颜色" ) ) {
                if( tab ) DrawColor();
            }

            using( var disabled = ImRaii.Disabled( !File.DyeTableEnabled ) )
            using( var tab = ImRaii.TabItem( "染色" ) ) {
                if( tab ) DyeRow.Draw();
            }

            if( PreviewDye != null ) {
                using var child = ImRaii.Child( "子级", new( -1, ImGui.GetFrameHeight() + ImGui.GetStyle().WindowPadding.Y * 2 ), true );
                using var style = ImRaii.PushStyle( ImGuiStyleVar.ItemSpacing, ImGui.GetStyle().ItemInnerSpacing );

                if( DyeData == null ) {
                    ImGui.TextDisabled( "[无染色值]" );
                }
                else {
                    var d = DyeData.Diffuse;
                    ImGui.ColorEdit3( "##Diffuse", ref d, ImGuiColorEditFlags.NoInputs | ImGuiColorEditFlags.DisplayRGB | ImGuiColorEditFlags.InputRGB | ImGuiColorEditFlags.NoTooltip );

                    ImGui.SameLine();
                    var s = DyeData.Specular;
                    ImGui.ColorEdit3( "##Specular", ref s, ImGuiColorEditFlags.NoInputs | ImGuiColorEditFlags.DisplayRGB | ImGuiColorEditFlags.InputRGB | ImGuiColorEditFlags.NoTooltip );

                    ImGui.SameLine();
                    var e = DyeData.Emissive;
                    ImGui.ColorEdit3( "##Emissive", ref s, ImGuiColorEditFlags.NoInputs | ImGuiColorEditFlags.DisplayRGB | ImGuiColorEditFlags.InputRGB | ImGuiColorEditFlags.NoTooltip );

                    using var disabled = ImRaii.Disabled();
                    ImGui.SameLine();
                    ImGui.SetNextItemWidth( 75 );
                    ImGui.InputFloat( "##Gloss", ref DyeData.Gloss, 0, 0, $"光泽: %.1f", ImGuiInputTextFlags.ReadOnly );
                    ImGui.SameLine();
                    ImGui.SetNextItemWidth( 75 );
                    ImGui.InputFloat( "##Power", ref DyeData.Gloss, 0, 0, $"强度: %.1f", ImGuiInputTextFlags.ReadOnly );
                }
            }

            if( Plugin.DirectXManager.MaterialPreview.CurrentRenderId != RenderId || editing.IsEdited ) {
                RefreshDye();
                Plugin.DirectXManager.MaterialPreview.LoadColorRow( this );
            }
            Plugin.DirectXManager.MaterialPreview.DrawInline();
        }
    }
}
