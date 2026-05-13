using VfxEditor.FileManager;
using VfxEditor.PhybFormat;
using VfxEditor.Utils;

namespace VfxEditor.Formats.PhybFormat {
    public class PhybManagerGroup : FileManagerGroup<PhybManager, PhybDocument, PhybFile, WorkspaceMetaBasic> {
        public PhybManagerGroup() : base( "Phyb Editor", "Phyb" ) { }

        protected override PhybManager GetNewManager() => new( this );
    }
}
