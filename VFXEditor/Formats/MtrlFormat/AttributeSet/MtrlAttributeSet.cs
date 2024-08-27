using System.Collections.Generic;
using System.IO;
using VfxEditor.Parsing;
using VfxEditor.Ui.Interfaces;
using VfxEditor.Utils;

namespace VfxEditor.Formats.MtrlFormat.AttributeSet {
    public class MtrlAttributeSet : IUiItem {
        public readonly ParsedString Name = new( "名称" );
        public readonly ParsedShort Index = new( "索引" );

        private readonly ushort TempOffset;

        public MtrlAttributeSet() { }

        public MtrlAttributeSet( BinaryReader reader ) {
            TempOffset = reader.ReadUInt16();
            Index.Read( reader );
        }

        public void ReadString( BinaryReader reader, long stringsStart ) {
            reader.BaseStream.Position = stringsStart + TempOffset;
            Name.Value = FileUtils.ReadString( reader );
        }

        public void Write( BinaryWriter writer, Dictionary<long, string> stringPositions ) {
            stringPositions[writer.BaseStream.Position] = Name.Value;
            writer.Write( ( ushort )0 ); // placeholder
            Index.Write( writer );
        }

        public void Draw() {
            Name.Draw();
            Index.Draw();
        }
    }
}
