using System.IO;
using VfxEditor.Formats.UldFormat.PartList;
using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Node.Data {
    public class ImageNodeData : UldGenericData {
        private readonly PartListSelect PartListId;
        private readonly PartItemSelect PartId;

        private readonly ParsedByteBool FlipH = new( "水平翻转" );
        private readonly ParsedByteBool FlipV = new( "垂直翻转" );
        private readonly ParsedInt Wrap = new( "换行", size: 1 );
        private readonly ParsedInt DrawMode = new( "绘制模式", size: 1 );

        public ImageNodeData() {
            PartListId = new();
            PartId = new( PartListId );
        }

        public override void Read( BinaryReader reader ) {
            PartListId.Read( reader );
            PartId.Read( reader );
            FlipH.Read( reader );
            FlipV.Read( reader );
            Wrap.Read( reader );
            DrawMode.Read( reader );
        }

        public override void Write( BinaryWriter writer ) {
            PartListId.Write( writer );
            PartId.Write( writer );
            FlipH.Write( writer );
            FlipV.Write( writer );
            Wrap.Write( writer );
            DrawMode.Write( writer );
        }

        public override void Draw() {
            PartListId.Draw();
            PartId.Draw();
            PartId.Selected?.DrawImage( false );

            FlipH.Draw();
            FlipV.Draw();
            Wrap.Draw();
            DrawMode.Draw();
        }
    }
}
