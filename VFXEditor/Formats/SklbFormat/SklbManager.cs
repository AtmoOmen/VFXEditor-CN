using VfxEditor.FileManager;
using VfxEditor.Interop.Havok;
using VfxEditor.Select.Formats;
using VfxEditor.Utils;

namespace VfxEditor.SklbFormat {
    public class SklbManager : FileManager<SklbDocument, SklbFile, WorkspaceMetaBasic> {
        public SklbManager() : base( "Sklb Editor", "Sklb" ) {
            SourceSelect = new SklbSelectDialog( "选择文件 [原始]##SKLB", this, true );
            ReplaceSelect = new SklbSelectDialog( "选择文件 [替换]##SKLB", this, false );
        }

        protected override SklbDocument GetNewDocument() => new( this, NewWriteLocation );

        protected override SklbDocument GetWorkspaceDocument( WorkspaceMetaBasic data, string localPath ) => new( this, NewWriteLocation, localPath, data );

        public bool GetSimpleSklb( string path, out SimpleSklb skeleton, out bool replaced ) {
            replaced = false;
            skeleton = null;

            // Local
            if( System.IO.Path.IsPathRooted( path ) ) {
                if( System.IO.Path.Exists( path ) ) {
                    skeleton = SimpleSklb.LoadFromLocal( path );
                    return true;
                }
                return false;
            }

            // Game file
            foreach( var document in Documents ) {
                if( document.File == null ) continue;
                if( document.ReplacePath.ToLower().Equals( path.ToLower() ) ) {
                    replaced = true;
                    skeleton = SimpleSklb.LoadFromLocal( document.WriteLocation );
                    return true;
                }
            }

            if( Dalamud.DataManager.FileExists( path ) ) {
                skeleton = Dalamud.DataManager.GetFile<SimpleSklb>( path );
                return true;
            }

            return false;
        }
    }
}
