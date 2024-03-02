using VfxEditor.Select.Tabs.Character;
using VfxEditor.Select.Tabs.Skeleton;
using VfxEditor.SklbFormat;

namespace VfxEditor.Select.Formats {
    public class SklbSelectDialog : SelectDialog {
        public SklbSelectDialog( string id, SklbManager manager, bool isSourceDialog ) : base( id, "sklb", manager, isSourceDialog ) {
            GameTabs.AddRange( new SelectTab[]{
                new SkeletonTabArmor( this, "装备", "skl", "sklb" ),
                new SkeletonTabNpc( this, "NPC" , "skl", "sklb"),
                new CharacterTabSkeleton( this, "角色", "skl", "sklb", true ),
                new SkeletonTabMount( this, "坐骑", "skl", "sklb")
            } );
        }
    }
}