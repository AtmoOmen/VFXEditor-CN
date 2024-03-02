using VfxEditor.FileManager;
using VfxEditor.Select.Formats;
using VfxEditor.Utils;

namespace VfxEditor.Formats.ShpkFormat {
    public unsafe class ShpkManager : FileManager<ShpkDocument, ShpkFile, WorkspaceMetaBasic> {
        public ShpkManager() : base( "Shpk Editor", "Shpk" ) {
            SourceSelect = new ShpkSelectDialog( "着色器选择 [加载]", this, true );
            ReplaceSelect = new ShpkSelectDialog( "着色器选择 [替换]", this, false );
        }

        protected override ShpkDocument GetNewDocument() => new( this, NewWriteLocation );

        protected override ShpkDocument GetWorkspaceDocument( WorkspaceMetaBasic data, string localPath ) => new( this, NewWriteLocation, localPath, data );
    }
}
