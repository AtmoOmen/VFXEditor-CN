﻿using System.IO;
using VfxEditor.Parsing;

namespace VfxEditor.ScdFormat {
    public class TrackModulationDepthFadeData : ScdTrackData {
        public readonly ParsedInt Carrier = new( "Carrier" );
        public readonly ParsedFloat Depth = new( "Depth" );
        public readonly ParsedInt FadeTime = new( "淡化时间" );

        public override void Read( BinaryReader reader ) {
            Carrier.Read( reader );
            Depth.Read( reader );
            FadeTime.Read( reader );
        }

        public override void Write( BinaryWriter writer ) {
            Carrier.Write( writer );
            Depth.Write( writer );
            FadeTime.Write( writer );
        }

        public override void Draw() {
            Carrier.Draw();
            Depth.Draw();
            FadeTime.Draw();
        }
    }
}
