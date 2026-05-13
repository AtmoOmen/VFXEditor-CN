using VfxEditor.FileManager;
using VfxEditor.Interop.Havok;
using VfxEditor.SklbFormat;
using VfxEditor.Utils;

namespace VfxEditor.Formats.SklbFormat {
    public class SklbManagerGroup : FileManagerGroup<SklbManager, SklbDocument, SklbFile, WorkspaceMetaBasic> {
        public SklbManagerGroup() : base( "Sklb Editor", "Sklb" ) { }

        protected override SklbManager GetNewManager() => new( this );

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
