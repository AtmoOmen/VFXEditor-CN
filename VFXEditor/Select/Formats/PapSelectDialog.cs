using VfxEditor.PapFormat;
using VfxEditor.Select.Tabs.Actions;
using VfxEditor.Select.Tabs.Character;
using VfxEditor.Select.Tabs.Common;
using VfxEditor.Select.Tabs.Emotes;
using VfxEditor.Select.Tabs.Event;
using VfxEditor.Select.Tabs.EventBase;
using VfxEditor.Select.Tabs.Items;
using VfxEditor.Select.Tabs.Job;
using VfxEditor.Select.Tabs.Mounts;
using VfxEditor.Select.Tabs.Npc;

namespace VfxEditor.Select.Formats {
    public class PapSelectDialog : SelectDialog {
        public PapSelectDialog( string id, PapManager manager, bool isSourceDialog ) : base( id, "pap", manager, isSourceDialog ) {
            GameTabs.AddRange( [
                new ItemTabPap( this, "武器" ),
                new ActionTabPap( this, "技能" ),
                new ActionTabPapNonPlayer( this, "NPC 技能" ),
                new EmoteTabPap( this, "表情" ),
                new NpcTabPap( this, "NPC" ),
                new MountTabPap( this, "坐骑" ),
                new CharacterTabPap( this, "角色" ),
                new JobTab( this, "职业" ),
                new EventTab (this, "过场剧情 (人类)" ),
                new EventBaseTab (this, "背景 NPC (人类)" ),
                new CommonTabPap (this, "一般" ),
            ] );
        }
    }
}