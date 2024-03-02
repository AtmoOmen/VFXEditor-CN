using Dalamud.Interface.Utility.Raii;
using ImGuiNET;
using System.Collections.Generic;
using VfxEditor.Select.Tabs.Actions;
using VfxEditor.Select.Tabs.Common;
using VfxEditor.Select.Tabs.Emotes;
using VfxEditor.Select.Tabs.Npc;
using VfxEditor.Spawn;
using VfxEditor.TmbFormat;

namespace VfxEditor.Select.Formats {
    public class TmbSelectDialog : SelectDialog {
        public TmbSelectDialog( string id, TmbManager manager, bool isSourceDialog ) : base( id, "tmb", manager, isSourceDialog ) {
            GameTabs.AddRange( new List<SelectTab>() {
                new ActionTabTmb( this, "技能" ),
                new ActionTabTmbNonPlayer( this, "非玩家对象动作" ),
                new EmoteTabTmb( this, "表情" ),
                new NpcTabTmb( this, "NPC" ),
                new CommonTabTmb( this, "通常" )
            } );
        }

        public override bool CanPlay => true;

        public override void PlayButton( string path ) {
            using var _ = ImRaii.PushId( "Spawn" );

            ImGui.SameLine();
            if( TmbSpawn.CanReset ) {
                if( ImGui.Button( "重置" ) ) TmbSpawn.Reset();
            }
            else {
                if( ImGui.Button( "播放" ) ) TmbSpawn.Apply( path );
            }
        }
    }
}