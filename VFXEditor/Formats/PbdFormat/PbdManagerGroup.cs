using VfxEditor.FileManager;
using VfxEditor.Utils;

namespace VfxEditor.Formats.PbdFormat {
    public class PbdManagerGroup : FileManagerGroup<PbdManager, PbdDocument, PbdFile, WorkspaceMetaBasic> {
        public PbdManagerGroup() : base( "Pbd Editor", "Pbd" ) { }

        protected override PbdManager GetNewManager() => new( this );
    }
}
