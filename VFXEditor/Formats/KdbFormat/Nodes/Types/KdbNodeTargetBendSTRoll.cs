using System.Collections.Generic;
using System.IO;

namespace VfxEditor.Formats.KdbFormat.Nodes.Types {
    public class KdbNodeTargetBendSTRoll : KdbNode {
        public KdbNodeTargetBendSTRoll() : base( KdbNodeType.TargetBendSTRoll ) { }

        public KdbNodeTargetBendSTRoll( BinaryReader reader ) : base( KdbNodeType.TargetBendSTRoll, reader ) { }

        public override void ReadBody( BinaryReader reader ) { }

        protected override List<KdbSlot> GetInputSlots() => [
            new( ConnectionType.Roll ),
            new( ConnectionType.BendS ),
            new( ConnectionType.BendT ),
        ];

        protected override List<KdbSlot> GetOutputSlots() => [];
    }
}
