using VfxEditor.FileManager;
using VfxEditor.Utils;

namespace VfxEditor.Formats.ShcdFormat {
    public class ShcdManagerGroup : FileManagerGroup<ShcdManager, ShcdDocument, ShcdFile, WorkspaceMetaBasic> {
        public ShcdManagerGroup() : base( "Shcd Editor", "Shcd" ) { }

        protected override ShcdManager GetNewManager() => new( this );
    }
}
