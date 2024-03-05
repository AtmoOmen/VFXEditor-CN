using VfxEditor.FileManager;
using VfxEditor.Select.Formats;
using VfxEditor.Utils;

namespace VfxEditor.PhybFormat {
    public class PhybManager : FileManager<PhybDocument, PhybFile, WorkspaceMetaBasic> {
        public PhybManager() : base( "Phyb Editor", "Phyb" ) {
            SourceSelect = new PhybSelectDialog( "选择文件 [加载]###PHYB", this, true );
            ReplaceSelect = new PhybSelectDialog( "选择文件 [替换]###PHYB", this, false );
        }

        protected override PhybDocument GetNewDocument() => new( this, NewWriteLocation );

        protected override PhybDocument GetWorkspaceDocument( WorkspaceMetaBasic data, string localPath ) => new( this, NewWriteLocation, localPath, data );
    }
}