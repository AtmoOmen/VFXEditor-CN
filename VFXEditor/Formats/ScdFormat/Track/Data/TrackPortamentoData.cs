﻿using System.IO;
using VfxEditor.Parsing;

namespace VfxEditor.ScdFormat {
    public class TrackPortamentoData : ScdTrackData {
        public readonly ParsedInt PortamentoTime = new( "Portamento Time" );
        public readonly ParsedFloat Pitch = new( "音高" );

        public override void Read( BinaryReader reader ) {
            PortamentoTime.Read( reader );
            Pitch.Read( reader );
        }

        public override void Write( BinaryWriter writer ) {
            PortamentoTime.Write( writer );
            Pitch.Write( writer );
        }

        public override void Draw() {
            PortamentoTime.Draw();
            Pitch.Draw();
        }
    }
}
