﻿using VfxEditor.Parsing;
using VfxEditor.TmbFormat.Utils;

namespace VfxEditor.TmbFormat {
    public class Tmdh : TmbItemWithId {
        public override string Magic => "TMDH";
        public override int Size => 0x10;
        public override int ExtraSize => 0;

        private readonly ParsedShort Unk1 = new( "未知 1" );
        private readonly ParsedShort Length = new( "长度" );
        private readonly ParsedShort Unk3 = new( "未知 3" );

        public Tmdh( TmbFile file, TmbReader reader ) : base( file, reader ) {
            Unk1.Read( reader );
            Length.Read( reader );
            Unk3.Read( reader );
        }

        public override void Write( TmbWriter writer ) {
            base.Write( writer );
            Unk1.Write( writer );
            Length.Write( writer );
            Unk3.Write( writer );
        }

        public void Draw() {
            Unk1.Draw();
            Length.Draw();
            Unk3.Draw();
        }
    }
}
