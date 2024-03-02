using Dalamud.Interface.Utility.Raii;
using ImGuiNET;
using System.Collections.Generic;

namespace VfxEditor.Select.Tabs.Character {
    public class SelectedPap {
        // Idle, MoveA, MoveB
        public Dictionary<string, string> General;

        // Pose # -> Start, Loop
        public Dictionary<string, Dictionary<string, string>> Poses;

        public Dictionary<string, string> FacePaths;

        public string GroundStart;
        public string Jmn;
        public Dictionary<string, Dictionary<string, string>> SitPoses;
    }

    public class CharacterTabPap : SelectTab<CharacterRow, SelectedPap> {
        public CharacterTabPap( SelectDialog dialog, string name ) : base( dialog, name, "Character", SelectResultType.GameCharacter ) { }

        // ===== LOADING =====

        public override void LoadData() => CharacterTab.Load( Items );

        public override void LoadSelection( CharacterRow item, out SelectedPap loaded ) {
            // General
            var general = new Dictionary<string, string>();
            var idlePath = item.GetPap( "resident/idle" );
            var movePathA = item.GetPap( "resident/move_a" );
            var movePathB = item.GetPap( "resident/move_b" );
            if( Dalamud.DataManager.FileExists( idlePath ) ) general.Add( "闲置动作", idlePath );
            if( Dalamud.DataManager.FileExists( movePathA ) ) general.Add( "移动动作 A", movePathA );
            if( Dalamud.DataManager.FileExists( movePathB ) ) general.Add( "移动动作 B", movePathB );

            // ===== STAND =======

            var poses = new Dictionary<string, Dictionary<string, string>>();
            for( var i = 1; i <= SelectDataUtils.MaxChangePoses; i++ ) {
                var start = item.GetStartPap( i, "" );
                var loop = item.GetLoopPap( i, "" );
                if( Dalamud.DataManager.FileExists( start ) && Dalamud.DataManager.FileExists( loop ) ) {
                    poses.Add( $"姿势 {i}", new Dictionary<string, string>() {
                        { "开始", start },
                        { "循环", loop }
                    } );
                }
            }

            // ======= SIT ========

            var sitPoses = new Dictionary<string, Dictionary<string, string>>();
            for( var i = 1; i <= SelectDataUtils.MaxChangePoses; i++ ) {
                var start = item.GetStartPap( i, "j_" );
                var loop = item.GetLoopPap( i, "j_" );
                if( Dalamud.DataManager.FileExists( start ) && Dalamud.DataManager.FileExists( loop ) ) {
                    sitPoses.Add( $"Sit Pose {i}", new Dictionary<string, string>() {
                        { "开始", start },
                        { "循环", loop }
                    } );
                }
            }

            var jmn = item.GetPap( "emote/jmn" );
            var groundStart = item.GetPap( "event_base/event_base_ground_start" );

            // ====== FACES ========

            var facePaths = new Dictionary<string, string>();
            foreach( var face in item.Data.FaceOptions ) {
                facePaths[$"面部 {face}"] = $"chara/human/{item.SkeletonId}/animation/f{face:D4}/resident/face.pap";
            }

            loaded = new SelectedPap {
                General = general,
                Poses = poses,
                SitPoses = sitPoses,
                FacePaths = SelectDataUtils.FileExistsFilter( facePaths ),
                Jmn = Dalamud.DataManager.FileExists( jmn ) ? jmn : null,
                GroundStart = Dalamud.DataManager.FileExists( groundStart ) ? groundStart : null,
            };
        }

        // ===== DRAWING ======

        protected override void DrawSelected() {
            using var tabBar = ImRaii.TabBar( "栏" );
            if( !tabBar ) return;

            if( ImGui.BeginTabItem( "一般" ) ) {
                DrawPaths( Loaded.General, Selected.Name );
                ImGui.EndTabItem();
            }
            if( ImGui.BeginTabItem( "姿势" ) ) {
                DrawPaths( Loaded.Poses, Selected.Name );
                ImGui.EndTabItem();
            }
            if( ImGui.BeginTabItem( "Ground Sit" ) ) {
                DrawPath( "Ground Start", Loaded.GroundStart, Selected.Name );
                DrawPath( "Jmn", Loaded.Jmn, Selected.Name );
                DrawPaths( Loaded.SitPoses, Selected.Name );
                ImGui.EndTabItem();
            }
            if( ImGui.BeginTabItem( "Faces" ) ) {
                DrawPaths( Loaded.FacePaths, Selected.Name );
                ImGui.EndTabItem();
            }
        }

        protected override string GetName( CharacterRow item ) => item.Name;
    }
}