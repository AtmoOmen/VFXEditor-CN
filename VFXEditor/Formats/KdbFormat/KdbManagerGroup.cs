using VfxEditor.FileManager;
using VfxEditor.Utils;

namespace VfxEditor.Formats.KdbFormat {
    public class KdbManagerGroup : FileManagerGroup<KdbManager, KdbDocument, KdbFile, WorkspaceMetaBasic> {
        public KdbManagerGroup() : base( "Kdb Editor", "Kdb" ) { }

        protected override KdbManager GetNewManager() => new( this );
    }
}
