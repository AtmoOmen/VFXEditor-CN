using System.Collections.Generic;
using VfxEditor.Formats.ShcdFormat;
using VfxEditor.Select.Tabs.Common;

namespace VfxEditor.Select.Formats {
    public class ShcdSelectDialog : SelectDialog {
        public ShcdSelectDialog( string id, ShcdManager manager, bool isSourceDialog ) : base( id, "shcd", manager, isSourceDialog ) {
            GameTabs.AddRange( new List<SelectTab>() {
                new CommonTabShader( this, "通常", "Shcd-Common", SelectDataUtils.CommonShcdPath, ".shcd" )
            } );
        }
    }
}