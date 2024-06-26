﻿using System.IO;
using VfxEditor.Parsing;
using VfxEditor.Ui.Interfaces;

namespace VfxEditor.Formats.MdlFormat.Box {
    public class MdlBoundingBox : IUiItem {
        public readonly ParsedFloat4 Min = new( "最小值" );
        public readonly ParsedFloat4 Max = new( "最大值" );

        public MdlBoundingBox() { }

        public MdlBoundingBox( BinaryReader reader ) : this() {
            Min.Read( reader );
            Max.Read( reader );
        }

        public virtual void Draw() {
            Min.Draw();
            Max.Draw();
        }

        public void Write( BinaryWriter writer ) {
            Min.Write( writer );
            Max.Write( writer );
        }
    }
}
