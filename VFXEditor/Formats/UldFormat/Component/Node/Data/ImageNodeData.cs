using System.IO;
using VfxEditor.Parsing;
using VfxEditor.Parsing.Int;
using VfxEditor.UldFormat.PartList;

namespace VfxEditor.UldFormat.Component.Node.Data {
    public class ImageNodeData : UldGenericData {
        private readonly ParsedIntSelect<UldPartList> PartListId;
        private readonly ParsedUIntPicker<UldPartItem> PartId;

        private readonly ParsedUInt Unknown1 = new( "未知 1", size: 2 );
        private readonly ParsedByteBool FlipH = new( "Flip H" );
        private readonly ParsedByteBool FlipV = new( "Flip V" );
        private readonly ParsedInt Wrap = new( "Wrap", size: 1 );
        private readonly ParsedInt DrawMode = new( "Draw Mode", size: 1 );

        public ImageNodeData() {
            PartListId = new( "组件列表", 0,
                () => Plugin.UldManager.File.PartsSplitView,
                ( UldPartList item ) => ( int )item.Id.Value,
                ( UldPartList item, int _ ) => item.GetText(),
                size: 2
            );
            PartId = new( "组件",
                () => PartListId.Selected?.Parts,
                ( UldPartItem item, int idx ) => item.GetText( idx ),
                null
            );
        }

        public override void Read( BinaryReader reader ) {
            PartListId.Read( reader );
            Unknown1.Read( reader );
            PartId.Read( reader );
            FlipH.Read( reader );
            FlipV.Read( reader );
            Wrap.Read( reader );
            DrawMode.Read( reader );
        }

        public override void Write( BinaryWriter writer ) {
            PartListId.Write( writer );
            Unknown1.Write( writer );
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
            Unknown1.Draw();
            DrawMode.Draw();
        }
    }
}
