﻿using HelixToolkit.SharpDX.Core;
using HelixToolkit.SharpDX.Core.Animations;
using SharpDX;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Parsing;
using VfxEditor.Parsing.String;

namespace VfxEditor.PhybFormat.Collision.NormalPlane {
    public class PhybNormalPlane : PhybData, IPhysicsObject {
        public readonly ParsedPaddedString Name = new( "名称", "replace_me", 32, 0xFE );
        public readonly ParsedPaddedString Bone = new( "骨骼", 32, 0xFE );
        public readonly ParsedFloat3 BoneOffset = new( "骨骼偏移" );
        public readonly ParsedFloat3 Normal = new( "法线" );
        public readonly ParsedFloat Thickness = new( "厚度" );

        public PhybNormalPlane( PhybFile file ) : base( file ) { }

        public PhybNormalPlane( PhybFile file, BinaryReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => [
            Name,
            Bone,
            BoneOffset,
            Normal,
            Thickness,
        ];

        public void AddPhysicsObjects( MeshBuilders meshes, Dictionary<string, Bone> boneMatrixes ) {
            if( !boneMatrixes.TryGetValue( Bone.Value, out var bone ) ) return;
            var offset = new Vector3( BoneOffset.Value.X, BoneOffset.Value.Y, BoneOffset.Value.Z );
            var pos = Vector3.Transform( offset, bone.BindPose ).ToVector3();

            var normal = new Vector3( Normal.Value.X, Normal.Value.Y, Normal.Value.Z ).Normalized();
            var tangent = Vector3.Cross( normal, Vector3.UnitY );
            if( tangent.Length() == 0 ) {
                tangent = Vector3.Cross( normal, Vector3.UnitX );
            }

            meshes.Collision.AddBox( pos, normal, tangent.Normalized(), 0.5f, 0.5f, Thickness.Value );
        }
    }
}
