using Dalamud.Interface.Utility.Raii;
using ImGuiNET;
using System.Collections.Generic;
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
            GameTabs.AddRange( new List<SelectTab>() {
                new ItemTabVfx( this, "Item" ),
                new StatusTabVfx( this, "状态" ),
                new ActionTabVfx( this, "技能" ),
                new ActionTabVfxNonPlayer( this, "非玩家对象动作" ),
                new EmoteTabVfx( this, "表情" ),
                new ZoneTabVfx( this, "区域" ),
                new GimmickTab( this, "机制" ),
                new HousingTab( this, "房屋" ),
                new NpcTabVfx( this, "NPC" ),
                new MountTabVfx( this, "坐骑" ),
                new CutsceneTab( this, "过场动画" ),
                new JournalCutsceneTab( this, "可回放过场动画" ),
                new CommonTabVfx( this, "通常" )
            } );
        }

        public override bool CanPlay => true;

        public override void PlayButton( string path ) {
            using var _ = ImRaii.PushId( "Spawn" );

            ImGui.SameLine();
            VfxSpawn.DrawButton( path, false );
        }

        public override void PlayPopupItems( string path ) {
            ImGui.Separator();

            if( ImGui.Selectable( "生成" ) ) VfxSpawn.OnSelf( path, false );
        }
    }
}