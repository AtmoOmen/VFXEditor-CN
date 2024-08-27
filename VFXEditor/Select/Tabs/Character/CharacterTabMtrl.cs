using Dalamud.Interface.Utility.Raii;
using ImGuiNET;
using System.Collections.Generic;
using System.Linq;

namespace VfxEditor.Select.Tabs.Character {
    public class SelectedMtrl {
        public Dictionary<(string, uint), List<string>> Faces;
        public Dictionary<string, List<string>> Bodies;
        public Dictionary<(string, uint), List<string>> Hairs;
        public Dictionary<(string, uint), List<string>> Ears;
        public Dictionary<(string, uint), List<string>> Tails;
    }

    public class CharacterTabMtrl : SelectTab<CharacterRow, SelectedMtrl> {
        public CharacterTabMtrl( SelectDialog dialog, string name ) : base( dialog, name, "Character" ) { }

        // ===== LOADING =====

        public override void LoadData() => CharacterTab.Load( Items );

        public override void LoadSelection( CharacterRow item, out SelectedMtrl loaded ) {
            loaded = new() {
                Faces = GetPart( "面部", CharacterPart.Face, item, item.Data.FaceOptions, ["fac_a", "etc_a", "iri_a"], item.Data.FaceToIcon ),
                Bodies = GetPart( "身体", CharacterPart.Body, item, item.Data.BodyOptions, ["a"] ),
                Hairs = GetPart( "头发", CharacterPart.Hair, item, item.Data.HairOptions, ["hir_a", "acc_b"], item.Data.HairToIcon ),
                Ears = GetPart( "耳朵", CharacterPart.Ear, item, item.Data.EarOptions, ["fac_a", "a"], item.Data.FeatureToIcon ),
                Tails = GetPart( "尾巴", CharacterPart.Tail, item, item.Data.TailOptions, ["a"], item.Data.FeatureToIcon )
            };
        }

        // ===== DRAWING ======

        protected override void DrawSelected() {
            using var tabBar = ImRaii.TabBar( "栏" );
            if( !tabBar ) return;

            if( ImGui.BeginTabItem( "面部" ) ) {
                Dialog.DrawPaths( Loaded.Faces, Selected.Name, SelectResultType.GameCharacter );
                ImGui.EndTabItem();
            }
            if( ImGui.BeginTabItem( "身体" ) ) {
                Dialog.DrawPaths( Loaded.Bodies, Selected.Name, SelectResultType.GameCharacter );
                ImGui.EndTabItem();
            }
            if( ImGui.BeginTabItem( "头发" ) ) {
                Dialog.DrawPaths( Loaded.Hairs, Selected.Name, SelectResultType.GameCharacter );
                ImGui.EndTabItem();
            }
            if( ImGui.BeginTabItem( "耳朵" ) ) {
                Dialog.DrawPaths( Loaded.Ears, Selected.Name, SelectResultType.GameCharacter );
                ImGui.EndTabItem();
            }
            if( ImGui.BeginTabItem( "尾巴" ) ) {
                Dialog.DrawPaths( Loaded.Tails, Selected.Name, SelectResultType.GameCharacter );
                ImGui.EndTabItem();
            }
        }

        private static IEnumerable<(int, List<string>)> IdsToPaths( IEnumerable<int> ids, IEnumerable<string> suffixes, CharacterRow item, CharacterPart part ) =>
            ids.Select( id =>
                (id, suffixes
                .Select( suffix => item.GetMtrl( part, id, suffix ) )
                .Where( path => Dalamud.DataManager.FileExists( path ) )
                .ToList()
                )
            )
            .Where( x => x.Item2.Count > 0 );

        private static Dictionary<string, List<string>> GetPart( string name, CharacterPart part, CharacterRow item, IEnumerable<int> ids, IEnumerable<string> suffixes ) =>
            IdsToPaths( ids, suffixes, item, part )
            .ToDictionary( x => $"{name} {x.Item1}", x => x.Item2 );

        private static Dictionary<(string, uint), List<string>> GetPart( string name, CharacterPart part, CharacterRow item, IEnumerable<int> ids, IEnumerable<string> suffixes, Dictionary<int, uint> iconMap ) =>
            IdsToPaths( ids, suffixes, item, part )
            .ToDictionary( x => ($"{name} {x.Item1}", iconMap.TryGetValue( x.Item1, out var icon ) ? icon : 0), x => x.Item2 );
    }
}
