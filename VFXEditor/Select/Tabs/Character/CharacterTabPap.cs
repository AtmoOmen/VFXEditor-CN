using Dalamud.Interface.Utility.Raii;
using Dalamud.Bindings.ImGui;
using System.Collections.Generic;
using System.Linq;

namespace VfxEditor.Select.Tabs.Character {
    public class SelectedPap {
        // Idle, MoveA, MoveB
        public Dictionary<string, string> General;

        // Pose # -> Start, Loop
        public Dictionary<string, Dictionary<string, string>> Poses;

        public List<(string, uint, string)> FacePaths;

        public string GroundStart;
        public string Jmn;
        public Dictionary<string, Dictionary<string, string>> SitPoses;
    }

    public class CharacterTabPap : SelectTab<CharacterRow, SelectedPap> {
        public CharacterTabPap( SelectDialog dialog, string name ) : base( dialog, name, "Character" ) { }

        // ===== LOADING =====

        public override void LoadData() => CharacterTab.Load( Items );

        public override void LoadSelection( CharacterRow item, out SelectedPap loaded ) {
            var general = new Dictionary<string, string>();
            var idlePath = item.GetPap( "resident/idle" );
            var movePathA = item.GetPap( "resident/move_a" );
            var movePathB = item.GetPap( "resident/move_b" );
            if( Dalamud.DataManager.FileExists( idlePath ) ) general.Add( "Idle", idlePath );
            if( Dalamud.DataManager.FileExists( movePathA ) ) general.Add( "Move A", movePathA );
            if( Dalamud.DataManager.FileExists( movePathB ) ) general.Add( "Move B", movePathB );

            var poses = new Dictionary<string, Dictionary<string, string>>();
            for( var i = 1; i <= SelectDataUtils.MaxChangePoses; i++ ) {
                var start = item.GetStartPap( i, "" );
                var loop = item.GetLoopPap( i, "" );
                if( Dalamud.DataManager.FileExists( start ) && Dalamud.DataManager.FileExists( loop ) ) {
                    poses.Add( $"姿势 {i}", new Dictionary<string, string>() {
                        { "开始", start },
                        { "Loop", loop }
                    } );
                }
            }

            var sitPoses = new Dictionary<string, Dictionary<string, string>>();
            for( var i = 1; i <= SelectDataUtils.MaxChangePoses; i++ ) {
                var start = item.GetStartPap( i, "j_" );
                var loop = item.GetLoopPap( i, "j_" );
                if( Dalamud.DataManager.FileExists( start ) && Dalamud.DataManager.FileExists( loop ) ) {
                    sitPoses.Add( $"坐姿 {i}", new Dictionary<string, string>() {
                        { "开始", start },
                        { "循环", loop }
                    } );
                }
            }

            var jmn = item.GetPap( "emote/jmn" );
            var groundStart = item.GetPap( "event_base/event_base_ground_start" );

            var facePaths = item.Data.FaceOptions
                .Select( id => (id, $"chara/human/{item.SkeletonId}/animation/f{id:D4}/resident/face.pap") )
                .Where( x => Dalamud.DataManager.FileExists( x.Item2 ) )
                .Select( x => ($"面部 {x.id}", item.Data.FaceToIcon.TryGetValue( x.id, out var icon ) ? icon : 0, x.Item2) )
                .ToList();

            loaded = new SelectedPap {
                General = general,
                Poses = poses,
                SitPoses = sitPoses,
                FacePaths = facePaths,
                Jmn = Dalamud.DataManager.FileExists( jmn ) ? jmn : null,
                GroundStart = Dalamud.DataManager.FileExists( groundStart ) ? groundStart : null,
            };
        }

        // ===== DRAWING ======

        protected override void DrawSelected() {
            using var tabBar = ImRaii.TabBar( "栏" );
            if( !tabBar ) return;

            if( ImGui.BeginTabItem( "一般" ) ) {
                Dialog.DrawPaths( Loaded.General, Selected.Name, SelectResultType.GameCharacter );
                ImGui.EndTabItem();
            }
            if( ImGui.BeginTabItem( "姿势" ) ) {
                Dialog.DrawPaths( Loaded.Poses, Selected.Name, SelectResultType.GameCharacter );
                ImGui.EndTabItem();
            }
            if( ImGui.BeginTabItem( "坐姿" ) ) {
                Dialog.DrawPaths( new Dictionary<string, string>() {
                    { "地面开始", Loaded.GroundStart },
                    { "Jmn", Loaded.Jmn },
                }, Selected.Name, SelectResultType.GameCharacter );

                ImGui.Separator();
                Dialog.DrawPaths( Loaded.SitPoses, Selected.Name, SelectResultType.GameCharacter );

                ImGui.EndTabItem();
            }
            if( ImGui.BeginTabItem( "面部" ) ) {
                Dialog.DrawPaths( Loaded.FacePaths, Selected.Name, SelectResultType.GameCharacter );
                ImGui.EndTabItem();
            }
        }
    }
}