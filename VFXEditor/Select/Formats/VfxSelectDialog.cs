using Dalamud.Interface;
using Dalamud.Interface.Utility.Raii;
using ImGuiNET;
using VfxEditor.AvfxFormat;
using VfxEditor.Select.Tabs.Actions;
using VfxEditor.Select.Tabs.Common;
using VfxEditor.Select.Tabs.Cutscenes;
using VfxEditor.Select.Tabs.Emotes;
using VfxEditor.Select.Tabs.Gimmick;
using VfxEditor.Select.Tabs.Housing;
using VfxEditor.Select.Tabs.Items;
using VfxEditor.Select.Tabs.JournalCutscene;
using VfxEditor.Select.Tabs.Mounts;
using VfxEditor.Select.Tabs.Npc;
using VfxEditor.Select.Tabs.Statuses;
using VfxEditor.Select.Tabs.Zone;
using VfxEditor.Spawn;

namespace VfxEditor.Select.Formats {
    public class VfxSelectDialog : SelectDialog {
        public VfxSelectDialog( string id, AvfxManager manager, bool isSourceDialog ) : base( id, "avfx", manager, isSourceDialog ) {
            GameTabs.AddRange( [
                new ItemTabVfx( this, "物品" ),
                new ItemTabVfxAccessory( this, "时尚配饰"),
                new StatusTabVfx( this, "状态" ),
                new ActionTabVfx( this, "技能" ),
                new ActionTabVfxNonPlayer( this, "NPC 技能" ),
                new EmoteTabVfx( this, "情感动作" ),
                new ZoneTabVfx( this, "坐骑" ),
                new GimmickTab( this, "机制" ),
                new HousingTab( this, "房屋" ),
                new NpcTabVfx( this, "NPC" ),
                new MountTabVfx( this, "坐骑" ),
                new CutsceneTab( this, "过场剧情" ),
                new JournalCutsceneTab( this, "可回放过场剧情" ),
                new CommonTabVfx( this, "一般" )
            ] );
        }

        public override bool CanPlay => true;

        public override void PlayButton( string path ) {
            using var font = ImRaii.PushFont( UiBuilder.IconFont );

            if( VfxSpawn.IsActive ) {
                if( ImGui.Button( FontAwesomeIcon.Pause.ToIconString() ) ) VfxSpawn.Clear();
            }
            else {
                if( ImGui.Button( FontAwesomeIcon.Play.ToIconString() ) ) VfxSpawn.OnSelf( path, false );
            }
        }

        public override void PlayPopupItems( string path ) {
            ImGui.Separator();
            if( ImGui.Selectable( "生成" ) ) VfxSpawn.OnSelf( path, false );
        }
    }
}