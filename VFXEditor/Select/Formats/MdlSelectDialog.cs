using VfxEditor.Formats.MdlFormat;
using VfxEditor.Select.Tabs.Character;
using VfxEditor.Select.Tabs.Items;
using VfxEditor.Select.Tabs.Npc;

namespace VfxEditor.Select.Formats {
    public class MdlSelectDialog : SelectDialog {
        public MdlSelectDialog( string id, MdlManager manager, bool isSourceDialog ) : base( id, "mdl", manager, isSourceDialog ) {
            GameTabs.AddRange( [
                new ItemTabMdl( this, "物品" ),
                new CharacterTabMdl( this, "角色" ),
                new NpcTabMdl( this, "NPC" ),
            ] );
        }
    }
}
