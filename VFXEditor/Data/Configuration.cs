using Dalamud.Configuration;
using Dalamud.Interface;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Bindings.ImGui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using VfxEditor.DirectX.Lights;
using VfxEditor.FileBrowser.SideBar;
using VfxEditor.Formats.TextureFormat;
using VfxEditor.Library;
using VfxEditor.Select;
using VfxEditor.SklbFormat.Bones;
using VfxEditor.Ui;
using VfxEditor.Utils.Gltf;

namespace VfxEditor {
    [Serializable]
    public unsafe class ManagerConfiguration {
        public List<SelectResult> RecentItems = [];
        public List<SelectResult> Favorites = [];
        public bool UseCustomWindowColor = false;
        public Vector4 TitleBg = *ImGui.GetStyleColorVec4( ImGuiCol.TitleBg );
        public Vector4 TitleBgActive = *ImGui.GetStyleColorVec4( ImGuiCol.TitleBgActive );
        public Vector4 TitleBgCollapsed = *ImGui.GetStyleColorVec4( ImGuiCol.TitleBgCollapsed );
    }

    [Serializable]
    public class Configuration : DalamudWindow, IPluginConfiguration {
        public int Version { get; set; } = 0;
        public bool IsEnabled { get; set; } = true;

        public bool LogAllFiles = false;
        public bool LogDebug = false;
        public bool LogVfxDebug = false;
        public bool LogVfxTriggers = false;

        public bool AutosaveEnabled = false;
        public int AutosaveSeconds = 300;
        public int SaveRecentLimit = 100;
        public bool AutosaveBackups = true;
        public int BackupCount = 20;

        public bool LockMainWindows = false;
        public bool HideWithUI = true;
        public bool ShowTabBar = true;
        public bool DocumentPopoutShowSource = false;
        public bool UseDegreesForAngles = true;

        public bool SelectDialogLogOpen = true;
        public bool SelectDialogIconsOnLeft = true;

        public bool OverlayLimit = true;
        public float OverlayRemoveDelay = 1;

        public bool BlockGameInputsWhenFocused = false;
        public string WriteLocation = Path.Combine( [
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "XIVLauncher",
            "pluginConfigs",
            "VFXEditor",
        ] );

        public string DefaultAuthor = "";

        public List<(string, string)> RecentWorkspaces = [];

        public bool VfxSpawnLoop = false;
        public float VfxSpawnDelay = 0.1f;

        public Dictionary<string, ManagerConfiguration> ManagerConfigs = [];

        public KeybindConfiguration SaveKeybind = new();
        public KeybindConfiguration SaveAsKeybind = new();
        public KeybindConfiguration OpenKeybind = new();
        public KeybindConfiguration UpdateKeybind = new();
        public KeybindConfiguration CopyKeybind = new();
        public KeybindConfiguration PasteKeybind = new();
        public KeybindConfiguration UndoKeybind = new();
        public KeybindConfiguration RedoKeybind = new();
        public KeybindConfiguration SpawnOnSelfKeybind = new();
        public KeybindConfiguration SpawnOnGroundKeybind = new();
        public KeybindConfiguration SpawnOnTargetKeybind = new();

        public List<LibraryProps> VFXNodeLibraryItems = [];
        public List<LibraryProps> VfxTextureLibraryItems = [];
        public List<LibraryProps> TmbTrackLibraryItems = [];
        public bool VfxTextureDefaultLoaded = false;

        public bool LoopMusic = true;
        public bool LoopSoundEffects = false;
        public float ScdVolume = 1f;
        public bool SimulateScdLoop = false;

        public Vector4 CurveEditorLineColor = new( 0, 0.1f, 1, 1 );
        public Vector4 CurveEditorPointColor = new( 1 );
        public Vector4 CurveEditorSelectedColor = new( 1.000f, 0.884f, 0.561f, 1f );
        public Vector4 CurveEditorPrimarySelectedColor = new( 0.984f, 0.726f, 0.011f, 1.0f );
        public List<Vector4> CurveEditorPalette = [];
        public int CurveEditorLineWidth = 2;
        public int CurveEditorColorRingSize = 3;
        public int CurveEditorGrabbingDistance = 25;
        public int CurveEditorPointSize = 7;
        public int CurveEditorSelectedSize = 10;
        public int CurveEditorPrimarySelectedSize = 12;
        public Vector4 TimelineSelectedColor = new( 1f, 0.532f, 0f, 1f );
        public Vector4 TimelineBarColor = new( 0.44f, 0.457f, 0.492f, 1f );

        public Vector4 FileBrowserSelectedColor = new( 1.000f, 0.884f, 0.561f, 1f );
        public Vector4 FileBrowserFolderColor = new( 0.516f, 0.859f, 1f, 1f );
        public Vector4 FileBrowserCodeColor = new( 0.229f, 1f, 0.832f, 1f );
        public Vector4 FileBrowserMiscColor = new( 1f, 0.789f, 0.233f, 1f );
        public Vector4 FileBrowserImageColor = new( 0.321f, 1f, 0.310f, 1f );
        public Vector4 FileBrowserFfxivColor = new( 1f, 0.543f, 0.508f, 1.0f );
        public Vector4 FileBrowserArchiveColor = new( 1f, 0.475f, 0.805f, 1.0f );
        public bool FileBrowserPreviewOpen = true;
        public bool FileBrowserImagePreview = true;
        public bool FileBrowserOverwriteDontAsk = false;
        public List<FileBrowserSidebarItem> FileBrowserRecent = [];
        public List<FileBrowserSidebarItem> FileBrowserFavorites = [];

        public Vector4 RendererBackground = new( 0.272f, 0.273f, 0.320f, 1.0f );
        public Vector3 MaterialAmbientColor = new( 0.0392f, 0.0156f, 0.04313f );
        public LightConfiguration Light1 = new( new( 2, 2, 2 ), new( 0.9153226f, 0.8648891f, 0.76768994f ), 10f, 0.1f );
        public LightConfiguration Light2 = new( new( -2, -2, -2 ), new( 0.39516127f, 0.28755587f, 0.2692833f ), 10f, 0.1f );

        public bool PhybSkeletonSplit = true;
        public bool EidSkeletonSplit = true;
        public bool ShowBoneNames = true;
        public BoneDisplay SklbBoneDisplay = BoneDisplay.Blender_Style_Perpendicular;
        public bool SklbMappingIndexDisplay = false;
        public bool SklbMappingUpdateExisting = true;
        public Vector4 SkeletonBoneNameColor = new( 1f );
        public Vector4 SkeletonBoneLineColor = new( 1f );

        public int PapMaterialDisplay = 1;
        public Vector3 PapMaterialBaseColor = new( 0, 0, 0 );

        public bool ModelWireframe = false;
        public bool ModelShowEdges = true;
        public bool ModelShowEmitters = true;
        public Vector2 ModelEmittersSize = new( 0.025f, 0.05f );

        public bool EmitterVertexSplitOpen = true;

        public Vector4 LuaParensColor = new( 0.5f, 0.5f, 0.5f, 1f );
        public Vector4 LuaFunctionColor = new( 0f, 0.439f, 1f, 1f );
        public Vector4 LuaLiteralColor = new( 0.639f, 0.207f, 0.933f, 1f );
        public Vector4 LuaVariableColor = new( 0.125f, 0.67058f, 0.45098f, 1f );

        public float NodeGraphUnitGridSmall = 10;
        public float NodeGraphUnitGridLarge = 50;
        public float NodeGraphGridSnapProximity = 3.5f;
        public float NodeGraphTimeForRulerTextFade = 2500;
        public bool NodeGraphShowRulerText = false;

        public List<ExcludedBonesConfiguration> ExcludedBones = [];

        public int PngMips = 9;
        public TextureFormat PngFormat = TextureFormat.DXT5;

        public string CustomPathPrefix = "vfx/custom/";

        [NonSerialized]
        public bool WriteLocationError = false;

        public Configuration() : base( "Settings", false, new( 300, 200 ), Plugin.WindowSystem ) { }

        public void Setup() {
            UpdateHideSettings();

            try { Directory.CreateDirectory( WriteLocation ); }
            catch( Exception ) { WriteLocationError = true; }

            Dalamud.Log( $"写入路径: {WriteLocation}" );

            if( CurveEditorPalette.Count == 0 ) {
                CurveEditorPalette.AddRange( ImGuiHelpers.DefaultColorPalette( 56 ) );
            }
        }

        public ManagerConfiguration GetManagerConfig( string id ) {
            if( ManagerConfigs.TryGetValue( id, out var config ) ) return config;
            var newConfig = new ManagerConfiguration();
            ManagerConfigs.Add( id, newConfig );
            return newConfig;
        }

        public void AddRecent( List<SelectResult> recentList, SelectResult result ) {
            if( result == null ) return;
            recentList.RemoveAll( result.CompareTo );

            recentList.Add( result );
            if( recentList.Count > SaveRecentLimit ) recentList.RemoveRange( 0, recentList.Count - SaveRecentLimit );
            Save();
        }

        public void AddRecentWorkspace( string path ) {
            RecentWorkspaces.RemoveAll( x => x.Item2 == path );
            var name = Path.GetFileName( path );
            RecentWorkspaces.Insert( 0, (name, path) );
            if( RecentWorkspaces.Count > 10 ) RecentWorkspaces.RemoveRange( 10, RecentWorkspaces.Count - 10 );
            Save();
        }

        public void Save() {
            Dalamud.PluginInterface.SavePluginConfig( this );
            UpdateHideSettings();
        }

        private void UpdateHideSettings() {
            Dalamud.PluginInterface.UiBuilder.DisableAutomaticUiHide = !HideWithUI;
            Dalamud.PluginInterface.UiBuilder.DisableUserUiHide = !HideWithUI;
            Dalamud.PluginInterface.UiBuilder.DisableCutsceneUiHide = !HideWithUI;
            Dalamud.PluginInterface.UiBuilder.DisableGposeUiHide = !HideWithUI;
        }

        // ======================

        public void AddFileBrowserRecent( string path ) {
            if( FileBrowserRecent.Any( x => x.Location == path ) ) return;

            FileBrowserRecent.Add( new FileBrowserSidebarItem {
                Icon = FontAwesomeIcon.Folder,
                Location = path,
                Text = Path.GetFileName( path )
            } );

            while( FileBrowserRecent.Count > 10 ) FileBrowserRecent.RemoveAt( 0 );
            Save();
        }

        public bool IsFileBrowserFavorite( string path ) => FileBrowserFavorites.Any( x => x.Location == path );

        public void RemoveFileBrowserFavorite( string path ) {
            FileBrowserFavorites.RemoveAll( x => x.Location == path );
            Save();
        }

        public void AddFileBrowserFavorite( string path ) {
            if( IsFileBrowserFavorite( path ) ) return;

            FileBrowserFavorites.Add( new FileBrowserSidebarItem {
                Icon = FontAwesomeIcon.Folder,
                Location = path,
                Text = Path.GetFileName( path )
            } );

            Save();
        }

        // =================

        public override void DrawBody() {
            using var _ = ImRaii.PushId( "Settings" );

            using var tabBar = ImRaii.TabBar( "栏" );
            if( !tabBar ) return;

            if( ImGui.BeginTabItem( "设置" ) ) {
                DrawConfiguration();
                ImGui.EndTabItem();
            }
            if( ImGui.BeginTabItem( "按键绑定" ) ) {
                DrawKeybinds();
                ImGui.EndTabItem();
            }
            if( ImGui.BeginTabItem( "浏览文件" ) ) {
                DrawFileBrowser();
                ImGui.EndTabItem();
            }
            if( ImGui.BeginTabItem( "3D 预览" ) ) {
                Draw3DView();
                ImGui.EndTabItem();
            }
            if( ImGui.BeginTabItem( "VFX" ) ) {
                DrawVfx();
                ImGui.EndTabItem();
            }
            if( ImGui.BeginTabItem( "TMB" ) ) {
                DrawTmb();
                ImGui.EndTabItem();
            }
            if( ImGui.BeginTabItem( "文件编辑器" ) ) {
                DrawEditorSpecific();
                ImGui.EndTabItem();
            }
        }

        private void DrawConfiguration() {
            using var child = ImRaii.Child( "Config" );

            if( ImGui.CollapsingHeader( "保存", ImGuiTreeNodeFlags.DefaultOpen ) ) {
                using var _ = ImRaii.PushIndent( 10f );
                ImGui.TextDisabled( "对缓存文件位置的修改可能需要通过重启游戏来生效" );
                if( ImGui.InputText( "写入位置", ref WriteLocation, 255 ) ) Save();

                if( ImGui.Checkbox( "自动保存工作区", ref AutosaveEnabled ) ) Save();
                using var disabled = ImRaii.Disabled( !AutosaveEnabled );
                using var indent = ImRaii.PushIndent();
                ImGui.SetNextItemWidth( 120 );
                if( ImGui.InputInt( "自动保存时间 (秒)", ref AutosaveSeconds ) ) Save();
                if( ImGui.Checkbox( "创建备份而非覆写", ref AutosaveBackups ) ) Save();
                using var disabled2 = ImRaii.Disabled( !AutosaveBackups );
                ImGui.SetNextItemWidth( 120 );
                if( ImGui.InputInt( "备份件数", ref BackupCount ) ) Save();
            }

            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );

            if( ImGui.CollapsingHeader( "日志", ImGuiTreeNodeFlags.DefaultOpen ) ) {
                using var _ = ImRaii.PushIndent( 10f );
                if( ImGui.Checkbox( "所有文件", ref LogAllFiles ) ) Save();
                if( ImGui.Checkbox( "DEBUG 信息", ref LogDebug ) ) Save();
                if( ImGui.Checkbox( "VFX DEBUG 信息", ref LogVfxDebug ) ) Save();
                if( ImGui.Checkbox( "VFX 触发信息", ref LogVfxTriggers ) ) Save();
            }

            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );

            if( ImGui.CollapsingHeader( "UI", ImGuiTreeNodeFlags.DefaultOpen ) ) {
                using var _ = ImRaii.PushIndent( 10f );
                if( ImGui.Checkbox( "跟随游戏 UI 隐藏", ref HideWithUI ) ) Save();
                if( ImGui.Checkbox( "显示操作栏", ref ShowTabBar ) ) Save();
                if( ImGui.Checkbox( "锁定主窗口", ref LockMainWindows ) ) Save();
                ImGui.SetNextItemWidth( 135 );
                if( ImGui.InputInt( "最近文件数量限制", ref SaveRecentLimit ) ) {
                    SaveRecentLimit = Math.Max( SaveRecentLimit, 0 );
                    Save();
                }
                ImGui.SetNextItemWidth( 135 );
                if( ImGui.InputFloat( "悬浮窗移除延迟", ref OverlayRemoveDelay ) ) Save();
                if( ImGui.Checkbox( "悬浮窗限制距离", ref OverlayLimit ) ) Save();
                if( ImGui.Checkbox( "在选择窗口显示图标", ref SelectDialogIconsOnLeft ) ) Save();
                if( ImGui.InputText( "默认 MOD 作者", ref DefaultAuthor, 255 ) ) Save();
            }
        }

        private void DrawKeybinds() {
            if( ImGui.Checkbox( "插件窗口聚焦时阻止按键传递至游戏", ref BlockGameInputsWhenFocused ) ) Save();

            using var child = ImRaii.Child( "按键绑定", new Vector2( -1 ), false );

            if( SaveKeybind.Draw( "保存" ) ) Save();
            if( SaveAsKeybind.Draw( "另存为" ) ) Save();
            if( OpenKeybind.Draw( "打开" ) ) Save();
            if( CopyKeybind.Draw( "复制" ) ) Save();
            if( PasteKeybind.Draw( "粘贴" ) ) Save();
            if( UndoKeybind.Draw( "撤销" ) ) Save();
            if( RedoKeybind.Draw( "重做" ) ) Save();
            if( UpdateKeybind.Draw( "刷新" ) ) Save();
            if( SpawnOnSelfKeybind.Draw( "生成于自身 (仅视效)" ) ) Save();
            if( SpawnOnGroundKeybind.Draw( "生成于地面 (仅视效)" ) ) Save();
            if( SpawnOnTargetKeybind.Draw( "生成于目标 (仅视效)" ) ) Save();
        }

        private void DrawVfx() {
            using var child = ImRaii.Child( "Vfx" );

            if( ImGui.CollapsingHeader( "曲线编辑器", ImGuiTreeNodeFlags.DefaultOpen ) ) {
                using var indent = ImRaii.PushIndent( 10f );

                if( ImGui.ColorEdit4( "线颜色", ref CurveEditorLineColor ) ) Save();
                if( ImGui.ColorEdit4( "点颜色", ref CurveEditorPointColor ) ) Save();
                if( ImGui.ColorEdit4( "首选颜色", ref CurveEditorPrimarySelectedColor ) ) Save();
                if( ImGui.ColorEdit4( "选中颜色", ref CurveEditorSelectedColor ) ) Save();

                if( ImGui.InputInt( "线宽度", ref CurveEditorLineWidth ) ) Save();
                if( ImGui.InputInt( "颜色环宽度", ref CurveEditorColorRingSize ) ) Save();
                if( ImGui.InputInt( "点大小", ref CurveEditorPointSize ) ) Save();
                if( ImGui.InputInt( "首选大小", ref CurveEditorPrimarySelectedSize ) ) Save();
                if( ImGui.InputInt( "选中大小", ref CurveEditorSelectedSize ) ) Save();
                if( ImGui.InputInt( "拾取距离", ref CurveEditorGrabbingDistance ) ) Save();
            }

            if( ImGui.CollapsingHeader( "时间线编辑器", ImGuiTreeNodeFlags.DefaultOpen ) ) {
                using var indent = ImRaii.PushIndent( 10f );

                if( ImGui.ColorEdit4( "选中颜色", ref TimelineSelectedColor ) ) Save();
                if( ImGui.ColorEdit4( "工具栏颜色", ref TimelineBarColor ) ) Save();
            }
        }

        private void DrawTmb() {
            using var child = ImRaii.Child( "Tmb" );

            if( ImGui.CollapsingHeader( "Lua", ImGuiTreeNodeFlags.DefaultOpen ) ) {
                using var indent = ImRaii.PushIndent( 10f );

                if( ImGui.ColorEdit4( "括号颜色", ref LuaParensColor ) ) Save();
                if( ImGui.ColorEdit4( "函数颜色", ref LuaFunctionColor ) ) Save();
                if( ImGui.ColorEdit4( "文本颜色", ref LuaLiteralColor ) ) Save();
                if( ImGui.ColorEdit4( "变量颜色", ref LuaVariableColor ) ) Save();
            }
        }

        private void DrawFileBrowser() {
            using var child = ImRaii.Child( "FileBrowser" );

            if( ImGui.Checkbox( "预览图像", ref FileBrowserImagePreview ) ) Save();
            if( ImGui.Checkbox( "跳过文件覆写确认", ref FileBrowserOverwriteDontAsk ) ) Save();

            if( ImGui.ColorEdit4( "选中颜色", ref FileBrowserSelectedColor ) ) Save();
            if( ImGui.ColorEdit4( "文件夹颜色", ref FileBrowserFolderColor ) ) Save();
            if( ImGui.ColorEdit4( "代码文件颜色", ref FileBrowserCodeColor ) ) Save();
            if( ImGui.ColorEdit4( "杂项文件颜色", ref FileBrowserMiscColor ) ) Save();
            if( ImGui.ColorEdit4( "图像文件颜色", ref FileBrowserImageColor ) ) Save();
            if( ImGui.ColorEdit4( "存档文件颜色", ref FileBrowserArchiveColor ) ) Save();
            if( ImGui.ColorEdit4( "FFXIV 文件颜色", ref FileBrowserFfxivColor ) ) Save();
        }

        private void Draw3DView() {
            using var child = ImRaii.Child( "3D View" );

            DrawDirectXCommon();

            if( ImGui.CollapsingHeader( "骨骼" ) ) {
                using var _ = ImRaii.PushIndent( 10f );
                DrawDirectXSkeleton();
            }

            if( ImGui.CollapsingHeader( "材质" ) ) {
                using var _ = ImRaii.PushIndent( 10f );
                DrawDirectXMaterials();
            }

            if( ImGui.CollapsingHeader( "VFX" ) ) {
                using var _ = ImRaii.PushIndent( 10f );
                DrawDirectXVfx();
            }
        }

        public void DrawDirectXCommon() {
            if( ImGui.ColorEdit4( "背景颜色", ref RendererBackground ) ) {
                Plugin.DirectXManager.Redraw();
                Save();
            }
        }

        public void DrawDirectXSkeleton() {
            if( ImGui.ColorEdit4( "骨骼名称颜色", ref SkeletonBoneNameColor ) ) Save();
            if( ImGui.ColorEdit4( "连接线颜色", ref SkeletonBoneLineColor ) ) Save();
        }

        public void DrawDirectXMaterials() {
            var updated = false;
            updated |= ImGui.ColorEdit3( "背景颜色", ref MaterialAmbientColor );

            if( ImGui.CollapsingHeader( "光照 1" ) ) {
                using var _ = ImRaii.PushIndent( 10f );
                using var __ = ImRaii.PushId( "Light1" );
                updated |= Light1.Draw();
            }

            if( ImGui.CollapsingHeader( "光照 2" ) ) {
                using var _ = ImRaii.PushIndent( 10f );
                using var __ = ImRaii.PushId( "Light2" );
                updated |= Light2.Draw();
            }

            if( updated ) {
                Plugin.DirectXManager.RedrawMaterials();
                Save();
            }
        }

        public void DrawDirectXVfx() {
            if( ImGui.InputFloat2( "预览金字塔大小", ref ModelEmittersSize ) ) {
                Save();
                Plugin.DirectXManager.ModelPreview.UpdatePyramidMesh();
                Plugin.DirectXManager.ModelPreview.Draw();
            }
        }

        private void DrawEditorSpecific() {
            using var child = ImRaii.Child( "EditorSpecific" );

            foreach( var config in ManagerConfigs ) {
                using var _ = ImRaii.PushId( config.Key );

                if( ImGui.CollapsingHeader( config.Key ) ) {
                    using var indent = ImRaii.PushIndent( 5f );

                    ImGui.Checkbox( "使用自定义窗口颜色", ref config.Value.UseCustomWindowColor );
                    if( config.Value.UseCustomWindowColor ) {
                        if( ImGui.ColorEdit4( "背景色", ref config.Value.TitleBg ) ) Save();
                        if( ImGui.ColorEdit4( "激活时的颜色", ref config.Value.TitleBgActive ) ) Save();
                        if( ImGui.ColorEdit4( "折叠时的颜色", ref config.Value.TitleBgCollapsed ) ) Save();
                    }
                }
            }
        }
    }
}