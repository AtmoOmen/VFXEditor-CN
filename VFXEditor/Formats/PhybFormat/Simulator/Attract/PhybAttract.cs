﻿using HelixToolkit.SharpDX.Core.Animations;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Parsing;
using VfxEditor.Parsing.String;

namespace VfxEditor.PhybFormat.Simulator.Attract {
    public class PhybAttract : PhybData, IPhysicsObject {
        public readonly PhybSimulator Simulator;

        public readonly ParsedPaddedString BoneName = new( "骨骼名", 32, 0xFE );
        public readonly ParsedFloat3 BoneOffset = new( "骨骼偏移" );
        public readonly ParsedShort ChainId = new( "链 ID" );
        public readonly ParsedShort NodeId = new( "节点 ID" );
        public readonly ParsedFloat Stiffness = new( "刚度" );

        public PhybAttract( PhybFile file, PhybSimulator simulator ) : base( file ) {
            Simulator = simulator;
        }

        public PhybAttract( PhybFile file, PhybSimulator simulator, BinaryReader reader ) : base( file, reader ) {
            Simulator = simulator;
        }

        protected override List<ParsedBase> GetParsed() => [
            BoneName,
            BoneOffset,
            ChainId,
            NodeId,
            Stiffness,
        ];

        public void AddPhysicsObjects( MeshBuilders meshes, Dictionary<string, Bone> boneMatrixes ) {
            Simulator.ConnectNodeToBone( ChainId.Value, NodeId.Value, BoneName.Value, meshes.Spring, boneMatrixes );
        }
    }
}
