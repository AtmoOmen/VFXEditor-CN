using Lumina.Excel.Sheets;
using System.Collections.Generic;
using VfxEditor.Select.Base;
using VfxEditor.Select.Tabs.Npc;

namespace VfxEditor.Select.Tabs.Mounts {
    public class MountRow : NpcRow, ISelectItemWithIcon {
        public readonly ushort Icon;
        public readonly string Bgm;
        public readonly int Seats;
        public readonly int SePack;

        // chara/human/[character ID]/animation/a0001/mt-[monster ID]/resident/mount.pap
        // chara/human/c0101/animation/a0001/mt_m0457/resident/mount01.pap

        // chara/monster/m0536/animation/a0001/bt_common/resident/mount.pap
        // sound/battle/mon/3388.scd

        public string Sound => $"sound/battle/mon/{SePack}.scd";

        public string Pap => $"chara/{PathPrefix}/{ModelString}/animation/a0001/bt_common/resident/mount.pap";

        public MountRow( Mount mount ) : base( mount.ModelChara.Value, mount.Singular.ToString() ) {
            Icon = mount.Icon;
            Bgm = mount.RideBGM.ValueNullable?.File.ToString();
            Seats = mount.ExtraSeats + 1;
            SePack = mount.ModelChara.ValueNullable?.SEPack ?? 0;
        }

        public List<string> GetSeatPaps() {
            var ret = new List<string>();
            if( Seats == 1 ) ret.Add( $"mt_{ModelString}/resident/mount.pap" );
            else {
                for( var i = 1; i <= Seats; i++ ) ret.Add( $"mt_{ModelString}/resident/mount0{i}.pap" );
            }
            return ret;
        }

        public uint GetIconId() => Icon;
    }
}