using System.Numerics;
using VfxEditor.FileManager;
using VfxEditor.Select.Formats;
using VfxEditor.Utils;

namespace VfxEditor.Formats.MtrlFormat {
    public class MtrlStain {
        public string Name;
        public uint Id;
        public Vector3 Color;
    }

    public unsafe class MtrlManager : FileManager<MtrlDocument, MtrlFile, WorkspaceMetaBasic> {
        public MtrlManager( MtrlManagerGroup group ) : base( group ) {
            SourceSelect = new MtrlSelectDialog( "Mtrl Select [LOADED]", this, true );
            ReplaceSelect = new MtrlSelectDialog( "Mtrl Select [REPLACED]", this, false );
        }

        protected override MtrlDocument GetNewDocument() => new( this, NewWriteLocation );

        protected override MtrlDocument GetWorkspaceDocument( WorkspaceMetaBasic data, string localPath ) => new( this, NewWriteLocation, localPath, data );
    }
}
