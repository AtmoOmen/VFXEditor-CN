﻿using VfxEditor.Formats.SkpFormat;
using VfxEditor.Select.Tabs.Character;
using VfxEditor.Select.Tabs.Skeleton;

namespace VfxEditor.Select.Formats {
    public class SkpSelectDialog : SelectDialog {
        public SkpSelectDialog( string id, SkpManager manager, bool isSourceDialog ) : base( id, "skp", manager, isSourceDialog ) {
            GameTabs.AddRange( [
                new SkeletonTabNpc( this, "NPC", "skl", "skp"),
                new CharacterTabSkeleton( this, "角色", "skl", "skp", false )
            ] );
        }
    }
}