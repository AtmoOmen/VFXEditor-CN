using FFXIVClientStructs.Havok.Animation.Animation;
using FFXIVClientStructs.Havok.Animation.Motion;
using FFXIVClientStructs.Havok.Animation.Rig;
using FFXIVClientStructs.Havok.Common.Base.Math.QsTransform;
using FFXIVClientStructs.Havok.Common.Base.Types;
using SharpGLTF.Animations;
using SharpGLTF.Scenes;
using SharpGLTF.Schema2;
using SharpGLTF.Transforms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using VfxEditor.Interop;
using VfxEditor.Interop.Havok;
using VfxEditor.Interop.Havok.Structs;
using VfxEditor.PapFormat.Motion;

namespace VfxEditor.Utils.Gltf {
    public class AnimationKeys {
        public readonly Dictionary<float, Vector3> ScaleKeys = [];
        public readonly Dictionary<float, Quaternion> RotateKeys = [];
        public readonly Dictionary<float, Vector3> TranslationKeys = [];
    }

    // There's something weird going on with the direct roots (n_hara, n_throw)

    public static unsafe class GltfAnimation {
        public static void ExportAnimation( hkaSkeleton* skeleton, List<string> animationNames, List<PapMotion> motions, bool skipUnanimated, string path ) {
            var scene = new SceneBuilder();

            var names = new List<string>();
            for( var i = 0; i < skeleton->Bones.Length; i++ ) {
                names.Add( skeleton->Bones[i].Name.String );
            }

            var dummyMesh = GltfSkeleton.GetDummyMesh();

            var bones = new List<NodeBuilder>();
            var roots = new List<NodeBuilder>();

            for( var i = 0; i < skeleton->Bones.Length; i++ ) {
                var node = new NodeBuilder( skeleton->Bones[i].Name.String );
                var pose = skeleton->ReferencePose[i];
                var pos = new Vector3( pose.Translation.X, pose.Translation.Y, pose.Translation.Z );
                var rot = new Quaternion( pose.Rotation.X, pose.Rotation.Y, pose.Rotation.Z, pose.Rotation.W );
                var scl = new Vector3( pose.Scale.X, pose.Scale.Y, pose.Scale.Z );
                node.SetLocalTransform( new AffineTransform( scl, rot, pos ), false );
                bones.Add( node );
            }

            for( var i = 0; i < skeleton->Bones.Length; i++ ) {
                var parentIdx = skeleton->ParentIndices[i];
                if( parentIdx != -1 ) {
                    bones[parentIdx].AddNode( bones[i] );
                }
                else {
                    roots.Add( bones[i] );
                }
            }

            scene.AddSkinnedMesh( dummyMesh, Matrix4x4.Identity, [.. bones] );
            var armature = new NodeBuilder( "Armature" );
            roots.ForEach( armature.AddNode );
            scene.AddNode( armature );

            var model = scene.ToGltf2();

            for( var animIdx = 0; animIdx < animationNames.Count; animIdx++ ) {
                var animationName = animationNames[animIdx];
                var motion = motions[animIdx];

                var nameToKeys = new Dictionary<string, AnimationKeys>();
                for( var i = 0; i < skeleton->Bones.Length; i++ ) {
                    nameToKeys[skeleton->Bones[i].Name.String] = new();
                }

                var animation = model.UseAnimation( animationName );
                for( var time = 0f; time <= motion.Duration; time += 1 / 30f ) {
                    ExportKeys( nameToKeys, names, motion, time );
                }

                var unanimated = skipUnanimated ? motion.GetUnanimatedBones() : null;

                var nodes = model.LogicalNodes;
                foreach( var node in nodes ) {
                    if( node.Name == null || !nameToKeys.ContainsKey( node.Name ) ) continue;
                    if( skipUnanimated ) {
                        var idx = names.IndexOf( node.Name );
                        if( unanimated.Contains( idx ) ) {
                            Dalamud.Log( $"正在跳过 {animationName} 中的无动画节点 {node.Name}" );
                            continue;
                        }
                    }

                    var keys = nameToKeys[node.Name];
                    animation.CreateRotationChannel( node, keys.RotateKeys, true );
                    animation.CreateScaleChannel( node, keys.ScaleKeys, true );
                    animation.CreateTranslationChannel( node, keys.TranslationKeys, true );
                }
            }

            model.SaveGLTF( path );
            Dalamud.Log( $"已保存 GLTF 至: {path}" );
        }

        private static void ExportKeys( Dictionary<string, AnimationKeys> nameToKeys, List<string> names, PapMotion motion, float time ) {
            var resetTime = motion.AnimationControl->LocalTime;
            motion.AnimationControl->LocalTime = time;

            var skeleton = motion.AnimatedSkeleton;

            var transforms = ( hkQsTransformf* )Marshal.AllocHGlobal( skeleton->Skeleton->Bones.Length * sizeof( hkQsTransformf ) );
            var floats = ( float* )Marshal.AllocHGlobal( skeleton->Skeleton->FloatSlots.Length * sizeof( float ) );
            skeleton->sampleAndCombineAnimations( transforms, floats );

            for( var i = 0; i < names.Count; i++ ) {
                var name = names[i];
                var key = nameToKeys[name];

                var transform = transforms[i];
                var pos = transform.Translation;
                var rot = transform.Rotation;
                var scl = transform.Scale;

                key.TranslationKeys[time] = new( pos.X, pos.Y, pos.Z );
                key.RotateKeys[time] = new( rot.X, rot.Y, rot.Z, rot.W );
                key.ScaleKeys[time] = new( scl.X, scl.Y, scl.Z );
            }

            // Reset
            Marshal.FreeHGlobal( ( nint )transforms );
            Marshal.FreeHGlobal( ( nint )floats );
            motion.AnimationControl->LocalTime = resetTime;
        }

        // Lord have mercy
        // https://github.com/0ceal0t/BlenderAssist/blob/main/BlenderAssist/pack_anim.cpp#L13

        public static void ImportAnimation(
            HashSet<nint> handles,
            hkaSkeleton* skeleton,
            PapMotion motion,
            int havokIndex,
            int gltfAnimationIndex,
            bool compress,
            bool skipUnanimated,
            List<string> excludeBones,
            string path
         ) {
            var model = ModelRoot.Load( path );
            var nodes = model.LogicalNodes;
            var animations = model.LogicalAnimations;

            var boneNames = new List<string>();
            var boneNameToIdx = new Dictionary<string, int>();
            var refPoses = new Dictionary<string, hkQsTransformf>();
            for( var i = 0; i < skeleton->Bones.Length; i++ ) {
                var name = skeleton->Bones[i].Name.String;
                boneNames.Add( name );
                boneNameToIdx[name] = i;
                refPoses[name] = skeleton->ReferencePose[i];
            }

            var skipBones = new List<string>();
            skipBones.AddRange( excludeBones );
            if( skipUnanimated ) {
                skipBones.AddRange( motion.GetUnanimatedBones().Select( x => boneNames[x] ) );
            }

            var tracks = new List<string>();
            foreach( var node in nodes ) {
                if( string.IsNullOrEmpty( node.Name ) ) continue;
                if( !boneNames.Contains( node.Name ) || !node.IsTransformAnimated || skipBones.Contains( node.Name ) ) {
                    Dalamud.Log( $"已跳过 gLTF 节点: {node.Name}" );
                    continue;
                }

                tracks.Add( node.Name );
            }

            if( animations.Count == 0 ) {
                Dalamud.Error( "文件无任何动画" );
                return;
            }

            var animation = animations[gltfAnimationIndex];
            var transforms = new List<hkQsTransformf>(); // final length will be numberOfFrames * tracks.Count

            var posSamplers = new Dictionary<string, ICurveSampler<Vector3>>();
            var rotSamplers = new Dictionary<string, ICurveSampler<Quaternion>>();
            var sclSamplers = new Dictionary<string, ICurveSampler<Vector3>>();

            foreach( var channel in animation.Channels ) {
                var name = channel.TargetNode.Name;
                var pos = channel.GetTranslationSampler()?.CreateCurveSampler();
                var rot = channel.GetRotationSampler()?.CreateCurveSampler();
                var scl = channel.GetScaleSampler()?.CreateCurveSampler();

                if( pos != null ) posSamplers[name] = pos;
                if( rot != null ) rotSamplers[name] = rot;
                if( scl != null ) sclSamplers[name] = scl;
            }

            var numberOfFrames = ( int )Math.Ceiling( animation.Duration * 30f ) + 1;
            for( var frame = 0; frame < numberOfFrames; frame++ ) {
                var time = frame * ( 1 / 30f );

                foreach( var track in tracks ) {
                    // TODO: or is this supposed to be identity matrix?
                    var refPose = refPoses[track];
                    var pos = refPose.Translation;
                    var rot = refPose.Rotation;
                    var scl = refPose.Scale;

                    // if we can find the samplers, use that
                    // otherwise use the ref pose

                    if( posSamplers.TryGetValue( track, out var posSampler ) ) {
                        var _pos = posSampler.GetPoint( time );
                        pos = new() {
                            X = _pos.X,
                            Y = _pos.Y,
                            Z = _pos.Z,
                            W = 1
                        };
                    }
                    if( rotSamplers.TryGetValue( track, out var rotSampler ) ) {
                        var _rot = rotSampler.GetPoint( time );
                        rot = new() {
                            X = GltfSkeleton.Cleanup( _rot.X ),
                            Y = GltfSkeleton.Cleanup( _rot.Y ),
                            Z = GltfSkeleton.Cleanup( _rot.Z ),
                            W = GltfSkeleton.Cleanup( _rot.W )
                        };
                    }

                    if( sclSamplers.TryGetValue( track, out var sclSampler ) ) {
                        var _scl = sclSampler.GetPoint( time );
                        scl = new() {
                            X = _scl.X,
                            Y = _scl.Y,
                            Z = _scl.Z,
                            W = 1
                        };
                    }

                    var transform = new hkQsTransformf() {
                        Translation = pos,
                        Rotation = rot,
                        Scale = scl
                    };

                    transforms.Add( transform );
                }
            }

            var currentBinding = motion.AnimationControl->Binding;
            var currentAnim = currentBinding.ptr->Animation;

            var anim = ( HkaInterleavedUncompressedAnimation* )Marshal.AllocHGlobal( Marshal.SizeOf<HkaInterleavedUncompressedAnimation>() );
            handles.Add( ( nint )anim );

            ( ( HkBaseObject* )anim )->vfptr = ( HkBaseObject.HkBaseObjectVtbl* )ResourceLoader.HavokInterleavedAnimationVtbl;

            var binding = ( hkaAnimationBinding* )Marshal.AllocHGlobal( Marshal.SizeOf<hkaAnimationBinding>() );
            handles.Add( ( nint )binding );

            ( ( HkBaseObject* )binding )->vfptr = ( ( HkBaseObject* )currentBinding.ptr )->vfptr;

            // Set up binding
            binding->OriginalSkeletonName = currentBinding.ptr->OriginalSkeletonName;
            binding->BlendHint = currentBinding.ptr->BlendHint;
            binding->PartitionIndices = currentBinding.ptr->PartitionIndices;

            var flags = currentBinding.ptr->TransformTrackToBoneIndices.Flags;

            binding->FloatTrackToFloatSlotIndices = HavokData.CreateArray( handles, currentBinding.ptr->FloatTrackToFloatSlotIndices, [] );
            binding->TransformTrackToBoneIndices = HavokData.CreateArray(
                handles, currentBinding.ptr->TransformTrackToBoneIndices, tracks.Select( x => ( short )boneNameToIdx[x] ).ToList() );

            // Set up animation
            anim->Animation.Type = hkaAnimation.AnimationType.InterleavedAnimation;
            anim->Animation.Duration = animation.Duration;
            anim->Animation.NumberOfTransformTracks = tracks.Count;
            anim->Animation.NumberOfFloatTracks = 0;
            anim->Animation.ExtractedMotion = new hkRefPtr<hkaAnimatedReferenceFrame> { ptr = null };
            anim->Animation.AnnotationTracks = HavokData.CreateArray( handles, ( uint )flags, new List<hkaAnnotationTrack>(), Marshal.SizeOf<hkaAnnotationTrack>() );
            anim->Floats = HavokData.CreateArray( handles, ( uint )flags, new List<float>(), sizeof( float ) );
            anim->Transforms = HavokData.CreateArray( handles, ( uint )flags, transforms, Marshal.SizeOf<hkQsTransformf>() );

            var finalAnim = ( hkaAnimation* )anim;

            if( compress ) {
                Dalamud.Log( "正在压缩动画..." );

                var spline = ( HkaSplineCompressedAnimation* )Marshal.AllocHGlobal( Marshal.SizeOf<HkaSplineCompressedAnimation>() );
                handles.Add( ( nint )spline );
                Plugin.ResourceLoader.HavokSplineCtor( spline, anim );
                finalAnim = ( hkaAnimation* )spline;
            }

            var animPtr = new hkRefPtr<hkaAnimation>() { ptr = finalAnim };
            var bindingPtr = new hkRefPtr<hkaAnimationBinding>() { ptr = binding };
            binding->Animation = animPtr;

            var container = motion.File.MotionData.AnimationContainer;
            var anims = HavokData.ToList( container->Animations );
            var bindings = HavokData.ToList( container->Bindings );
            anims[havokIndex] = animPtr;
            bindings[havokIndex] = bindingPtr;

            container->Animations = HavokData.CreateArray( handles, ( uint )container->Animations.Flags, anims, sizeof( nint ) );
            container->Bindings = HavokData.CreateArray( handles, ( uint )container->Bindings.Flags, bindings, sizeof( nint ) );

            container->Animations[havokIndex] = animPtr;
        }
    }
}
