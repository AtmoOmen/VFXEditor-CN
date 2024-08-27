using VfxEditor.PhybFormat;
using VfxEditor.Select.Tabs.Character;
using VfxEditor.Select.Tabs.Skeleton;

namespace VfxEditor.Select.Formats {
    public class PhybSelectDialog : SelectDialog {
        public PhybSelectDialog( string id, PhybManager manager, bool isSourceDialog ) : base( id, "phyb", manager, isSourceDialog ) {
            GameTabs.AddRange( [
                new SkeletonTabArmor( this, "装备", "phy", "phyb" ),
                new SkeletonTabWeapon( this, "武器", "phy", "phyb" ),
                new SkeletonTabNpc( this, "NPC" , "phy", "phyb"),
                new CharacterTabSkeleton( this, "角色", "phy", "phyb", true ),
                new SkeletonTabMount( this, "坐骑", "phy", "phyb")
            ] );
        }
    }
}