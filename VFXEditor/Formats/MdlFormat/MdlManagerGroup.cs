using VfxEditor.FileManager;
using VfxEditor.Utils;

namespace VfxEditor.Formats.MdlFormat {
    public class MdlManagerGroup : FileManagerGroup<MdlManager, MdlDocument, MdlFile, WorkspaceMetaBasic> {
        public MdlManagerGroup() : base( "Mdl Editor", "Mdl" ) { }

        protected override MdlManager GetNewManager() => new( this );
    }
}
