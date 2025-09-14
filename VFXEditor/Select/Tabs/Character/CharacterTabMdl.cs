using Dalamud.Interface.Utility.Raii;
using Dalamud.Bindings.ImGui;
using System.Collections.Generic;
using System.Linq;

namespace VfxEditor.Select.Tabs.Character {
    public class SelectedMdl {
        public List<(string, uint, string)> Faces;
        public List<(string, string)> Bodies;
        public List<(string, uint, string)> Hairs;
        public List<(string, uint, string)> Ears;
        public List<(string, uint, string)> Tails;
    }

    public class CharacterTabMdl : SelectTab<CharacterRow, SelectedMdl> {
        public CharacterTabMdl( SelectDialog dialog, string name ) : base( dialog, name, "Character" ) { }

        // ===== LOADING =====

        public override void LoadData() => CharacterTab.Load( Items );

        public override void LoadSelection( CharacterRow item, out SelectedMdl loaded ) {
            loaded = new() {
                Faces = GetPart( "面部", CharacterPart.Face, item, item.Data.FaceOptions, item.Data.FaceToIcon ),
                Bodies = GetPart( "身体", CharacterPart.Body, item, item.Data.BodyOptions ),
                Hairs = GetPart( "头发", CharacterPart.Hair, item, item.Data.HairOptions, item.Data.HairToIcon ),
                Ears = GetPart( "耳朵", CharacterPart.Ear, item, item.Data.EarOptions, item.Data.FeatureToIcon ),
                Tails = GetPart( "尾巴", CharacterPart.Tail, item, item.Data.TailOptions, item.Data.FeatureToIcon )
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

        private static List<(string, string)> GetPart( string name, CharacterPart part, CharacterRow item, IEnumerable<int> ids ) =>
            ids
            .Select( id => (id, item.GetMdl( part, id )) )
            .Where( x => Dalamud.DataManager.FileExists( x.Item2 ) )
            .Select( x => ($"{name} {x.id}", x.Item2) ).ToList();

        private static List<(string, uint, string)> GetPart( string name, CharacterPart part, CharacterRow item, IEnumerable<int> ids, Dictionary<int, uint> iconMap ) =>
            ids
            .Select( id => (id, item.GetMdl( part, id )) )
            .Where( x => Dalamud.DataManager.FileExists( x.Item2 ) )
            .Select( x => ($"{name} {x.id}", iconMap.TryGetValue( x.id, out var icon ) ? icon : 0, x.Item2) )
            .ToList();
    }
}
