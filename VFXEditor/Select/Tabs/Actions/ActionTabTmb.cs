using Dalamud.Interface;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;
using System.Linq;
using VfxEditor.Utils;

namespace VfxEditor.Select.Tabs.Actions {
    public class ActionTabTmb : SelectTab<ActionRow> {
        public ActionTabTmb( SelectDialog dialog, string name ) : this( dialog, name, "Action-Tmb" ) { }

        public ActionTabTmb( SelectDialog dialog, string name, string stateId ) : base( dialog, name, stateId, SelectResultType.GameAction ) { }

        // ===== LOADING =====

        public override void LoadData() {
            var sheet = Dalamud.DataManager.GetExcelSheet<Action>()
                .Where( x => !string.IsNullOrEmpty( x.Name ) && ( x.IsPlayerAction || x.ClassJob.Value != null ) && !x.AffectsPosition );
            foreach( var item in sheet ) Items.Add( new ActionRow( item ) );
        }

        // ===== DRAWING ======

        protected override void DrawSelected() {
            DrawIcon( Selected.Icon );
            DrawPath( "开始", Selected.StartPath, $"{Selected.Name} Start" );
            DrawMovementCancel( Selected.StartMotion );

            DrawPath( "结束", Selected.EndPath, $"{Selected.Name} End" );
            DrawMovementCancel( Selected.EndMotion );

            DrawPath( "命中", Selected.HitPath, $"{Selected.Name} Hit" );

            DrawPath( "武器", Selected.WeaponPath, $"{Selected.Name} Weapon" );
        }

        protected override string GetName( ActionRow item ) => item.Name;

        private void DrawMovementCancel( bool disabled ) {
            if( !disabled ) return;
            if( Dialog.ShowLocal ) return;
            ImGui.Indent( 25f );
            UiUtils.IconText( FontAwesomeIcon.QuestionCircle, true );
            UiUtils.Tooltip( "这一参数位于游戏的数据表格中，无法被 VFXEditor 移除" );
            ImGui.SameLine();
            ImGui.TextDisabled( "被移动取消的动画" );
            ImGui.Unindent( 25f );
        }
    }
}