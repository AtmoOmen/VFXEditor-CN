﻿using HelixToolkit.SharpDX.Core;
using HelixToolkit.SharpDX.Core.Animations;
using SharpDX;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Parsing;
using VfxEditor.Parsing.String;

namespace VfxEditor.PhybFormat.Collision.Sphere {
    public class PhybSphere : PhybData, IPhysicsObject {
        public readonly ParsedPaddedString Name = new( "名称", "replace_me", 32, 0xFE );
        public readonly ParsedPaddedString Bone = new( "骨骼", 32, 0xFE );
        public readonly ParsedFloat3 BoneOffset = new( "骨骼偏移" );
        public readonly ParsedFloat Thickness = new( "厚度" );

        public PhybSphere( PhybFile file ) : base( file ) { }

        public PhybSphere( PhybFile file, BinaryReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => [
            Name,
            Bone,
            BoneOffset,
            Thickness,
        ];

        public void AddPhysicsObjects( MeshBuilders meshes, Dictionary<string, Bone> boneMatrixes ) {
            if( !boneMatrixes.TryGetValue( Bone.Value, out var bone ) ) return;
            var offset = new Vector3( BoneOffset.Value.X, BoneOffset.Value.Y, BoneOffset.Value.Z );
            var pos = Vector3.Transform( offset, bone.BindPose ).ToVector3();
            meshes.Collision.AddSphere( pos, Thickness.Value / 2f, 10, 10 );
        }
    }
}
