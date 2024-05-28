using VfxEditor.ScdFormat;
using VfxEditor.Select.Tabs.Actions;
using VfxEditor.Select.Tabs.Bgm;
using VfxEditor.Select.Tabs.BgmQuest;
using VfxEditor.Select.Tabs.Common;
using VfxEditor.Select.Tabs.Instance;
using VfxEditor.Select.Tabs.Mounts;
using VfxEditor.Select.Tabs.Orchestrions;
using VfxEditor.Select.Tabs.Voice;
using VfxEditor.Select.Tabs.Zone;

namespace VfxEditor.Select.Formats {
    public class ScdSelectDialog : SelectDialog {
        public ScdSelectDialog( string id, ScdManager manager, bool isSourceDialog ) : base( id, "scd", manager, isSourceDialog ) {
            GameTabs.AddRange( [
                new ActionTabScd( this, "技能" ),
                new MountTabScd( this, "坐骑" ),
                new OrchestrionTab( this, "管弦乐琴音乐" ),
                new ZoneTabScd( this, "区域" ),
                new BgmTab( this, "BGM" ),
                new BgmQuestTab( this, "任务 BGM" ),
                new InstanceTab( this, "副本" ),
                new VoiceTab( this, "语音" ),
                new CommonTabScd( this, "一般" ),
            ] );
        }
    }
}
