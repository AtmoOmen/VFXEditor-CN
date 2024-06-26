using Lumina.Data.Files;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VfxEditor.Select.Tabs.Items {
    public class ItemTabPap : ItemTab<List<(string, string)>> {
        public ItemTabPap( SelectDialog dialog, string name ) : base( dialog, name, "Item-Pap", ItemTabFilter.Weapon ) { }

        // ===== LOADING =====

        public override void LoadSelection( ItemRow item, out List<(string, string)> loaded ) {
            loaded = [];

            // Motion animation
            if( item is not ItemRowWeapon weapon ) return;
            if( Dalamud.DataManager.FileExists( weapon.PapPath ) ) loaded.Add( ("Motion", weapon.PapPath) );

            // Material animation
            var imcPath = item.ImcPath;
            if( !Dalamud.DataManager.FileExists( imcPath ) ) return;
            try {
                var file = Dalamud.DataManager.GetFile<ImcFile>( imcPath );
                foreach( var id in file
                    .GetParts()
                    .Select( x => x.Variants[item.Variant - 1] )
                    .Where( x => x.MaterialAnimationId != 0 )
                    .Select( x => ( int )x.MaterialAnimationId ) ) {
                    loaded.Add( ($"Material {id}", weapon.GetMaterialPap( id )) );
                }
            }
            catch( Exception e ) {
                Dalamud.Error( e, "加载 IMC 文件失败 " + imcPath );
            }
        }

        // ===== DRAWING ======

        protected override void DrawSelected() {
            Dialog.DrawPaths( Loaded, Selected.Name, SelectResultType.GameItem );
        }
    }
}