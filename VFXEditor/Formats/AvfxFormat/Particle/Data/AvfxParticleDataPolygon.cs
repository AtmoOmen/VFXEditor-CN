﻿namespace VfxEditor.AvfxFormat {
    public class AvfxParticleDataPolygon : AvfxData {
        public readonly AvfxCurve Count = new( "数量", "Cnt" );

        public AvfxParticleDataPolygon() : base() {
            Parsed = [
                Count
            ];

            Tabs.Add( Count );
        }
    }
}
