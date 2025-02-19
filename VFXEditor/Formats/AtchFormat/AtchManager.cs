using VfxEditor.FileManager;
using VfxEditor.Select.Formats;
using VfxEditor.Utils;

namespace VfxEditor.Formats.AtchFormat {
    public unsafe class AtchManager : FileManager<AtchDocument, AtchFile, WorkspaceMetaBasic> {
        public AtchManager() : base( "Atch Editor", "Atch" ) {
            SourceSelect = new AtchSelectDialog( "选择文件 [原始]##ATCH", this, true );
            ReplaceSelect = new AtchSelectDialog( "选择文件 [替换]###ATCH", this, false );
        }

        protected override AtchDocument GetNewDocument() => new( this, NewWriteLocation );

        protected override AtchDocument GetWorkspaceDocument( WorkspaceMetaBasic data, string localPath ) => new( this, NewWriteLocation, localPath, data );
    }
}