using ImGuiNET;
using System.IO;
using System.Numerics;
using VfxEditor.Parsing;
using VfxEditor.Parsing.Int;
using VfxEditor.UldFormat.PartList;

namespace VfxEditor.UldFormat.Component.Node.Data {
    public enum GridPartsType : int {
        Divide = 0x0,
        Compose = 0x1,
    }

    public enum GridRenderType : int {
        Scale = 0x0,
        Tile = 0x1,
    }

    public class NineGridNodeData : UldGenericData {
        private readonly ParsedIntSelect<UldPartList> PartListId;
        private readonly ParsedUIntPicker<UldPartItem> PartId;

        private readonly ParsedUInt Unknown1 = new( "未知 1", size: 2 );
        private readonly ParsedEnum<GridPartsType> GridParts = new( "网格组件类型", size: 1 );
        private readonly ParsedEnum<GridRenderType> GridRender = new( "网格渲染类型", size: 1 );
        private readonly ParsedShort TopOffset = new( "顶部偏移" );
        private readonly ParsedShort BottonOffset = new( "底部偏移" );
        private readonly ParsedShort LeftOffset = new( "左侧偏移" );
        private readonly ParsedShort RightOffset = new( "右侧偏移" );
        private readonly ParsedInt Unknown2 = new( "未知 2", size: 1 );
        private readonly ParsedInt Unknown3 = new( "未知 3", size: 1 );

        public NineGridNodeData() {
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
            GridParts.Read( reader );
            GridRender.Read( reader );
            TopOffset.Read( reader );
            BottonOffset.Read( reader );
            LeftOffset.Read( reader );
            RightOffset.Read( reader );
            Unknown2.Read( reader );
            Unknown3.Read( reader );
        }

        public override void Write( BinaryWriter writer ) {
            PartListId.Write( writer );
            Unknown1.Write( writer );
            PartId.Write( writer );
            GridParts.Write( writer );
            GridRender.Write( writer );
            TopOffset.Write( writer );
            BottonOffset.Write( writer );
            LeftOffset.Write( writer );
            RightOffset.Write( writer );
            Unknown2.Write( writer );
            Unknown3.Write( writer );
        }

        public override void Draw() {
            PartListId.Draw();
            PartId.Draw();

            var part = PartId.Selected;
            if( part != null ) {
                ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );

                var mult = part.ShowHd ? 2f : 1f;
                var pos = ImGui.GetCursorScreenPos();
                var size = part.Size.Value;
                var width = size.X;
                var height = size.Y;

                part.DrawImage( false );
                var drawList = ImGui.GetWindowDrawList();
                var color = ImGui.ColorConvertFloat4ToU32( new( 1f, 0f, 0f, 1f ) );

                drawList.AddLine( pos + new Vector2( LeftOffset.Value, 0 ) * mult, pos + new Vector2( LeftOffset.Value, height ) * mult, color, 1f );
                drawList.AddLine( pos + new Vector2( width - RightOffset.Value, 0 ) * mult, pos + new Vector2( width - RightOffset.Value, height ) * mult, color, 1f );

                drawList.AddLine( pos + new Vector2( 0, TopOffset.Value ) * mult, pos + new Vector2( width, TopOffset.Value ) * mult, color, 1f );
                drawList.AddLine( pos + new Vector2( 0, height - BottonOffset.Value ) * mult, pos + new Vector2( width, height - BottonOffset.Value ) * mult, color, 1f );

                ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );
            }

            GridParts.Draw();
            GridRender.Draw();
            TopOffset.Draw();
            BottonOffset.Draw();
            LeftOffset.Draw();
            RightOffset.Draw();
            Unknown1.Draw();
            Unknown2.Draw();
            Unknown3.Draw();
        }
    }
}
