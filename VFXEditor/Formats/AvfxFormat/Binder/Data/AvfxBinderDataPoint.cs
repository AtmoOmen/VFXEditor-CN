﻿namespace VfxEditor.AvfxFormat {
    public class AvfxBinderDataPoint : AvfxData {
        public readonly AvfxCurve SpringStrength = new( "弹簧强度", "SpS" );

        public AvfxBinderDataPoint() : base() {
            Parsed = [
                SpringStrength
            ];

            Tabs.Add( SpringStrength );
        }
    }
}
