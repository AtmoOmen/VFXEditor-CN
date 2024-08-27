using VfxEditor.FileManager;
using VfxEditor.Select.Formats;
using VfxEditor.Utils;

namespace VfxEditor.Formats.MdlFormat {
    public unsafe class MdlManager : FileManager<MdlDocument, MdlFile, WorkspaceMetaBasic> {
        public MdlManager() : base( "Mdl Editor", "Mdl" ) {
            SourceSelect = new MdlSelectDialog( "选择文件 [加载]###MDL", this, true );
            ReplaceSelect = new MdlSelectDialog( "选择文件 [替换]###MDL", this, false );
        }

        protected override MdlDocument GetNewDocument() => new( this, NewWriteLocation );

        protected override MdlDocument GetWorkspaceDocument( WorkspaceMetaBasic data, string localPath ) => new( this, NewWriteLocation, localPath, data );
    }
}
