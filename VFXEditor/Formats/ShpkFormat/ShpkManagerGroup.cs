using VfxEditor.FileManager;
using VfxEditor.Utils;

namespace VfxEditor.Formats.ShpkFormat {
    public class ShpkManagerGroup : FileManagerGroup<ShpkManager, ShpkDocument, ShpkFile, WorkspaceMetaBasic> {
        public ShpkManagerGroup() : base( "Shpk Editor", "Shpk" ) { }

        protected override ShpkManager GetNewManager() => new( this );
    }
}
