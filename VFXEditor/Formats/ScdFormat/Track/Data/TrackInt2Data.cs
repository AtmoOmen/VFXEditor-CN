﻿using System.IO;
using VfxEditor.Parsing;

namespace VfxEditor.ScdFormat {
    public class TrackInt2Data : ScdTrackData {
        public readonly ParsedInt Value1 = new( "值 1" );
        public readonly ParsedInt Value2 = new( "值 2" );

        public override void Read( BinaryReader reader ) {
            Value1.Read( reader );
            Value2.Read( reader );
        }

        public override void Write( BinaryWriter writer ) {
            Value1.Write( writer );
            Value2.Write( writer );
        }

        public override void Draw() {
            Value1.Draw();
            Value2.Draw();
        }
    }
}
