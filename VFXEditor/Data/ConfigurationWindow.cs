using System;
using System.Collections.Generic;
using System.Text;
using VfxEditor.Ui;

namespace VfxEditor.Data {
    public class ConfigurationWindow : DalamudWindow {
        private readonly Configuration Configuration;

        public ConfigurationWindow( Configuration configuration ) : base( "VFXEditor Settings", false, new( 300, 500 ), Plugin.WindowSystem ) {
            Configuration = configuration;
        }

        public override void DrawBody() {
            Configuration.DrawBody();
        }
    }
}
