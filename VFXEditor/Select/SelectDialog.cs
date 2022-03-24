using ImGuiFileDialog;
using ImGuiNET;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace VFXSelect {
    public enum SelectResultType {
        Local,
        GamePath,
        GameItem,
        GameStatus,
        GameAction,
        GameZone,
        GameEmote,
        GameGimmick,
        GameCutscene,
        GameNpc
    }

    public struct SelectResult {
        public SelectResultType Type;
        public string DisplayString;
        public string Path;

        public SelectResult( SelectResultType type, string displayString, string path ) {
            Type = type;
            DisplayString = displayString;
            Path = path;
        }

        public static SelectResult None() {
            var s = new SelectResult {
                DisplayString = "[NONE]",
                Path = ""
            };
            return s;
        }
    }

    public abstract class SelectDialog {
        private readonly string Id;
        private readonly string Ext;
        private readonly bool ShowLocal;
        private readonly List<SelectResult> RecentList;

        private bool Visible = false;

        protected Action<SelectResult> OnSelect;

        protected abstract List<SelectTab> GetTabs();

        public SelectDialog( string id, string ext, List<SelectResult> recentList, bool showLocal, Action<SelectResult> onSelect ) {
            Id = id;
            Ext = ext;
            RecentList = recentList;
            ShowLocal = showLocal;
            OnSelect = onSelect;
        }

        public void Show() {
            Visible = true;
        }

        public void Hide() {
            Visible = false;
        }

        public void Draw() {
            if( !Visible ) return;
            ImGui.SetNextWindowSize( new Vector2( 800, 500 ), ImGuiCond.FirstUseEver );

            if( !ImGui.Begin( Id + "##" + Id, ref Visible, ImGuiWindowFlags.NoDocking ) ) {
                ImGui.End();
                return;
            }

            ImGui.BeginTabBar( "Tabs##" + Id );
            if( ShowLocal ) DrawLocal();
            DrawGamePath();
            DrawGame();
            if( RecentList != null ) DrawRecent();
            ImGui.EndTabBar();
            ImGui.End();
        }

        // =========== LOCAL ================
        private string LocalPath = "";
        private void DrawLocal() {
            var ret = ImGui.BeginTabItem( "Local File##Select-" + Id );
            if( !ret )
                return;

            var id = "##Select/Local/" + Id;
            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );
            ImGui.Text( $".{Ext} file located on your computer, eg: C:/Users/me/Downloads/awesome.{Ext}" );
            ImGui.Text( "Path" );
            ImGui.SameLine();
            ImGui.InputText( id + "Input", ref LocalPath, 255 );
            ImGui.SameLine();
            if( ImGui.Button( ( "Browse" + id ) ) ) {
                FileDialogManager.OpenFileDialog( "Select a File", $".{Ext},.*", ( bool ok, string res ) => {
                    if( !ok ) return;
                    Invoke( new SelectResult( SelectResultType.Local, "[LOCAL] " + res, res ) );
                } );
            }
            ImGui.SameLine();
            if( ImGui.Button( "SELECT" + id ) ) {
                Invoke( new SelectResult( SelectResultType.Local, "[LOCAL] " + LocalPath, LocalPath ) );
            }

            ImGui.EndTabItem();
        }

        // ============= GAME =================
        public void DrawGame() {
            var ret = ImGui.BeginTabItem( "Game Items##Select/" + Id );
            if( !ret ) return;

            ImGui.BeginTabBar( "GameSelectTabs##" + Id );
            foreach( var tab in GetTabs() ) {
                tab.Draw();
            }
            ImGui.EndTabBar();
            ImGui.EndTabItem();
        }

        // ============== GAME FILE ================
        private string GamePath = "";
        public void DrawGamePath() {
            var ret = ImGui.BeginTabItem( "Game Path##Select/" + Id );
            if( !ret )
                return;

            var id = "##Select/GamePath/" + Id;
            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );
            ImGui.Text( $"In-game .{Ext} file, eg: vfx/common/eff/wp_astro1h.{Ext}" );
            ImGui.Text( "Path" );
            ImGui.SameLine();
            ImGui.InputText( id + "Input", ref GamePath, 255 );
            ImGui.SameLine();
            if( ImGui.Button( "SELECT" + id ) ) {
                Invoke( new SelectResult( SelectResultType.GamePath, "[GAME] " + GamePath, GamePath ) );
            }

            ImGui.EndTabItem();
        }

        // ======== RECENT ========
        public SelectResult RecentSelected;
        public bool IsRecentSelected = false;
        public void DrawRecent() {
            var ret = ImGui.BeginTabItem( "Recent##Select/" + Id );
            if( !ret )
                return;
            var id = "##Recent/" + Id;

            var footerHeight = ImGui.GetStyle().ItemSpacing.Y + ImGui.GetFrameHeightWithSpacing();
            ImGui.BeginChild( id + "/Child", new Vector2( 0, -footerHeight ), true );
            var idx = 0;
            foreach( var item in RecentList ) {
                if( item.Type == SelectResultType.Local && !ShowLocal )
                    continue;

                if( ImGui.Selectable( item.DisplayString + id + idx, RecentSelected.Equals( item ) ) ) {
                    IsRecentSelected = true;
                    RecentSelected = item;
                }
                idx++;
            }
            ImGui.EndChild();
            if( IsRecentSelected ) {
                if( ImGui.Button( "SELECT" + id ) ) {
                    Invoke( RecentSelected );
                }
            }

            ImGui.EndTabItem();
        }

        public void Invoke( SelectResult result ) {
            OnSelect?.Invoke( result );
        }
    }
}