using VfxEditor.FileManager;
using VfxEditor.Utils;

namespace VfxEditor.Formats.AtchFormat {
    public class AtchManagerGroup : FileManagerGroup<AtchManager, AtchDocument, AtchFile, WorkspaceMetaBasic> {
        public AtchManagerGroup() : base( "Atch Editor", "Atch" ) { }

        protected override AtchManager GetNewManager() => new( this );
    }
}
