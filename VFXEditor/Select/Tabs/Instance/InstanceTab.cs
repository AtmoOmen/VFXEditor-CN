using Lumina.Excel.GeneratedSheets2;
using System.Linq;
using VfxEditor.Select.Tabs.BgmQuest;

namespace VfxEditor.Select.Tabs.Instance {
    public class SelectedInstance {
        public BgmSituationStruct Situation;
    }

    public class InstanceTab : SelectTab<InstanceRow, SelectedInstance> {
        public InstanceTab( SelectDialog dialog, string name ) : base( dialog, name, "Instance" ) { }

        // ===== LOADING =====

        public override void LoadData() {
            var sheet = Dalamud.DataManager.GetExcelSheet<ContentFinderCondition>().Where( x => !string.IsNullOrEmpty( x.Name ) && x.Content.Row > 0 && x.ContentLinkType == 1 );
            foreach( var item in sheet ) Items.Add( new InstanceRow( item ) );
        }

        public override void LoadSelection( InstanceRow item, out SelectedInstance loaded ) {
            var instance = Dalamud.DataManager.GetExcelSheet<InstanceContent>().GetRow( item.ContentRowId );
            loaded = new() {
                Situation = BgmQuestTab.GetBgmSituation( ( ushort )instance.BGM.Row )
            };
        }

        // ===== DRAWING ======

        protected override void DrawSelected() {
            DrawIcon( Selected.Image );
            Dialog.DrawBgmSituation( Selected.Name, Loaded.Situation, SelectResultType.GameMusic );
        }
    }
}