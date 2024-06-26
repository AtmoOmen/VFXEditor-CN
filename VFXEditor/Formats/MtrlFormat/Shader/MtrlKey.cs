﻿using System.IO;
using VfxEditor.Formats.ShpkFormat.Utils;
using VfxEditor.Ui.Interfaces;

namespace VfxEditor.Formats.MtrlFormat.Shader {
    public class MtrlKey : IUiItem {
        public readonly ParsedCrc Id = new( "Id" );
        public readonly ParsedCrc Value = new( "值" );

        public MtrlKey() { }

        public MtrlKey( uint id, uint defaultValue ) {
            Id.Value = id;
            Value.Value = defaultValue;
        }

        public MtrlKey( BinaryReader reader ) {
            Id.Read( reader );
            Value.Read( reader );
        }

        public void Write( BinaryWriter writer ) {
            Id.Write( writer );
            Value.Write( writer );
        }

        public void Draw() {
            Id.Draw( CrcMaps.Keys );
            Value.Draw( CrcMaps.Keys );
        }

        public string GetText( int idx ) => CrcMaps.Keys.TryGetValue( Id.Value, out var text ) ? text : $"键 {idx} (0x{Id.Value:X4})";
    }
}
