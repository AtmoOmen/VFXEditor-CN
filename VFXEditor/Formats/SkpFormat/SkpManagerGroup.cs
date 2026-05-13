using VfxEditor.FileManager;
using VfxEditor.Utils;

namespace VfxEditor.Formats.SkpFormat {
    public class SkpManagerGroup : FileManagerGroup<SkpManager, SkpDocument, SkpFile, WorkspaceMetaBasic> {
        public SkpManagerGroup() : base( "Skp Editor", "Skp" ) { }

        protected override SkpManager GetNewManager() => new( this );
    }
}
