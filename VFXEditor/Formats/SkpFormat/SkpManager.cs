using VfxEditor.FileManager;
using VfxEditor.Select.Formats;
using VfxEditor.Utils;

namespace VfxEditor.Formats.SkpFormat {
    public unsafe class SkpManager : FileManager<SkpDocument, SkpFile, WorkspaceMetaBasic> {
        public SkpManager() : base( "Skp Editor", "Skp" ) {
            SourceSelect = new SkpSelectDialog( "选择文件 [原始]##SKP", this, true );
            ReplaceSelect = new SkpSelectDialog( "选择文件 [替换]##SKP", this, false );
        }

        protected override SkpDocument GetNewDocument() => new( this, NewWriteLocation );

        protected override SkpDocument GetWorkspaceDocument( WorkspaceMetaBasic data, string localPath ) => new( this, NewWriteLocation, localPath, data );
    }
}
