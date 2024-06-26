using VfxEditor.Formats.MtrlFormat;
using VfxEditor.Select.Tabs.Character;
using VfxEditor.Select.Tabs.Items;

namespace VfxEditor.Select.Formats {
    public class MtrlSelectDialog : SelectDialog {
        public MtrlSelectDialog( string id, MtrlManager manager, bool isSourceDialog ) : base( id, "mtrl", manager, isSourceDialog ) {
            GameTabs.AddRange( [
                new ItemTabMtrl( this, "物品" ),
                new CharacterTabMtrl( this, "角色" ),
            ] );
        }
    }
}