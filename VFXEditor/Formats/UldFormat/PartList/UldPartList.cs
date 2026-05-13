using Dalamud.Interface.Utility.Raii;
using Dalamud.Bindings.ImGui;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Data.Command.ListCommands;
using VfxEditor.Utils;

namespace VfxEditor.UldFormat.PartList {
    public class UldPartList : UldWorkspaceItem {
        private readonly UldFile File;
        public readonly List<UldPartItem> Parts = [];

        private int Offset => 12 + Parts.Count * 12;

        public UldPartList( UldFile file, uint id ) : base( id ) {
            File = file;
        }

        public UldPartList( UldFile file, BinaryReader reader ) : this( file, 0 ) {
            Id.Read( reader );
            var partCount = reader.ReadInt32();
            reader.ReadInt32(); // skip offset

            for( var i = 0; i < partCount; i++ ) {
                Parts.Add( new UldPartItem( file, reader ) );
            }
        }

        public void Write( BinaryWriter writer ) {
            Id.Write( writer );
            writer.Write( Parts.Count );
            writer.Write( Offset );
            foreach( var part in Parts ) part.Write( writer );
        }

        public override void Draw() {
            DrawRename();
            Id.Draw();

            for( var idx = 0; idx < Parts.Count; idx++ ) {
                using var _ = ImRaii.PushId( idx );

                var item = Parts[idx];
                var currentTexture = item.CurrentTexture;
                var text = currentTexture != null ? currentTexture.GetText() : $"材质 {item.TextureId.Value}";

                if( ImGui.CollapsingHeader( $"分部 {idx} ({text})##{idx}" ) ) {
                    using var indent = ImRaii.PushIndent();

                    if( UiUtils.RemoveButton( "删除", true ) ) { // REMOVE
                        CommandManager.Add( new ListRemoveCommand<UldPartItem>( Parts, item ) );
                        break;
                    }

                    item.Draw();
                }
            }

            if( ImGui.Button( "+ New" ) ) { // NEW
                CommandManager.Add( new ListAddCommand<UldPartItem>( Parts, new UldPartItem( File ) ) );
            }
        }

        public override string GetDefaultText() => $"分部列表 {GetIdx()}";

        public override string GetWorkspaceId() => $"PartList{GetIdx()}";
    }
}
