using System.Collections.Generic;
using System.IO;

namespace VfxEditor.Formats.PbdFormat {
    public class PbdConnection {
        public readonly PbdDeformer Item;

        public PbdConnection Parent { get; set; }
        public PbdConnection Child { get; set; }
        public PbdConnection Sibling { get; set; }

        private readonly short _ParentIdx;
        private readonly short _ChildIdx;
        private readonly short _SiblingIdx;

        public PbdConnection( PbdDeformer item ) {
            Item = item;
        }

        public PbdConnection( List<PbdDeformer> deformers, BinaryReader reader ) {
            _ParentIdx = reader.ReadInt16();
            _ChildIdx = reader.ReadInt16();
            _SiblingIdx = reader.ReadInt16();
            Item = deformers[reader.ReadInt16()];
        }

        public void Populate( List<PbdConnection> connections ) {
            if( _ParentIdx >= 0 ) Parent = connections[_ParentIdx];
            if( _ChildIdx >= 0 ) Child = connections[_ChildIdx];
            if( _SiblingIdx >= 0 ) Sibling = connections[_SiblingIdx];
        }
    }
}