using Lumina.Excel.GeneratedSheets2;
using System.Linq;

namespace VfxEditor.Select.Tabs.Zone {
    public abstract class ZoneTab<T> : SelectTab<ZoneRow, T> where T : class {
        public ZoneTab( SelectDialog dialog, string name, string stateId ) : base( dialog, name, stateId ) { }

        // ===== LOADING =====

        public override void LoadData() {
            var sheet = Dalamud.DataManager.GetExcelSheet<TerritoryType>().Where( x => !string.IsNullOrEmpty( x.Name ) );
            foreach( var item in sheet ) Items.Add( new ZoneRow( item ) );
        }
    }
}