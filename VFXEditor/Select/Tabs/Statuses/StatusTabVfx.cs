using Lumina.Excel.Sheets;
using System.Collections.Generic;
using System.Linq;

namespace VfxEditor.Select.Tabs.Statuses
{
    public class StatusTabVfx : SelectTab<StatusRow>
    {
        public StatusTabVfx( SelectDialog dialog, string name ) : base( dialog, name, "Status-Vfx" ) { }
        
        // ===== LOADING =====

        public override void LoadData() {
            var sheet = Dalamud.DataManager.GetExcelSheet<Status>().Where( x => !string.IsNullOrEmpty( x.Name.ExtractText() ) );
            foreach( var item in sheet ) {
                var status = new StatusRow( item );
                if( status.VfxExists ) Items.Add( status );
            }
        }
        
        // ===== DRAWING ======
        
        protected override void DrawSelected()
        {
            var paths = new Dictionary<string, string>() {
                { "受击", Selected.HitPath },
            };
            
            foreach( var (path, idx) in Selected.LoopPaths.WithIndex() )
            {
                paths[$"循环 {idx + 1}"] = path;
            }
            
            Dialog.DrawPaths( paths, Selected.Name, SelectResultType.GameStatus );
        }
    }
}