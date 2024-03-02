using VfxEditor.EidFormat;
using VfxEditor.Select.Tabs.Character;
using VfxEditor.Select.Tabs.Skeleton;

namespace VfxEditor.Select.Formats {
    public class EidSelectDialog : SelectDialog {
        public EidSelectDialog( string id, EidManager manager, bool isSourceDialog ) : base( id, "eid", manager, isSourceDialog ) {
            GameTabs.AddRange( new SelectTab[]{
                new SkeletonTabNpc( this, "NPC", "eid", "eid"),
                new CharacterTabSkeleton( this, "角色", "eid", "eid", false ),
                new SkeletonTabMount( this, "坐骑", "eid", "eid"),
            } );
        }
    }
}