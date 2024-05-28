using VfxEditor.FileManager;
using VfxEditor.Select.Formats;
using VfxEditor.Utils;

namespace VfxEditor.Formats.ShpkFormat {
    public unsafe class ShpkManager : FileManager<ShpkDocument, ShpkFile, WorkspaceMetaBasic> {
        public ShpkManager() : base( "Shpk Editor", "Shpk" ) {
            SourceSelect = new ShpkSelectDialog( "选择文件 [加载]###SHPK", this, true );
            ReplaceSelect = new ShpkSelectDialog( "选择文件 [替换]###SHPK", this, false );
        }

        protected override ShpkDocument GetNewDocument() => new( this, NewWriteLocation );

        protected override ShpkDocument GetWorkspaceDocument( WorkspaceMetaBasic data, string localPath ) => new( this, NewWriteLocation, localPath, data );
    }
}
