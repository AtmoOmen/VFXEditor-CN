using Lumina.Excel.Sheets;
using System.Collections.Generic;
using System.Linq;

namespace VfxEditor.Select.Tabs.Actions
{
    public class ActionTabTmb : SelectTab<ActionRow>
    {
        public ActionTabTmb( SelectDialog dialog, string name ) : this( dialog, name, "Action-Tmb" ) { }
        
        public ActionTabTmb( SelectDialog dialog, string name, string stateId ) : base( dialog, name, stateId ) { }
        
        // ===== LOADING =====
        
        public override void LoadData()
        {
            var sheet = Dalamud.DataManager.GetExcelSheet<Action>()
                .Where( x => !string.IsNullOrEmpty( x.Name.ExtractText() ) && ( x.IsPlayerAction || x.ClassJob.ValueNullable != null ) && !x.AffectsPosition );
            foreach( var item in sheet ) Items.Add( new ActionRow( item ) );
        }
        
        // ===== DRAWING ======
        
        protected override void DrawSelected()
        {
            Dialog.DrawPaths( new Dictionary<string, string>() {
                { "开始",  Selected.StartTmbPath },
                { "结束",  Selected.EndTmbPath },
                { "受击",  Selected.HitTmbPath },
                { "武器",  Selected.WeaponTmbPath },
                
            }, Selected.Name, SelectResultType.GameAction );
        }
    }
}