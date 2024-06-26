﻿using FFXIVClientStructs.Havok;
using ImGuiNET;
using VfxEditor.Interop.Structs.Animation;
using VfxEditor.Parsing;
using VfxEditor.SklbFormat.Bones;
using VfxEditor.Ui.Interfaces;

namespace VfxEditor.SklbFormat.Mapping {
    public unsafe class SklbSimpleMapping : IUiItem {
        public readonly SklbMapping Mapping;

        public readonly ParsedBoneIndex BoneA = new( "骨骼节点映射", -1 );
        public readonly ParsedBoneIndex BoneB = new( "该骨骼节点", -1 );
        public readonly ParsedInt Unk1 = new( "未知 1" );
        public readonly ParsedInt Unk2 = new( "未知 2" );
        public readonly ParsedInt Unk3 = new( "未知 3" );
        public readonly ParsedFloat4 Translation = new( "平移" );
        public readonly ParsedQuat Rotation = new( "旋转", new( 0, 0, 0 ) );
        public readonly ParsedFloat4 Scale = new( "缩放", new( 1, 1, 1, 1 ) );

        public SklbSimpleMapping( SklbMapping mapping ) {
            Mapping = mapping;
        }

        public SklbSimpleMapping( SklbMapping mapping, SimpleMapping simpleMapping ) : this( mapping ) {
            BoneA.Value = simpleMapping.BoneA;
            BoneB.Value = simpleMapping.BoneB;
            Unk1.Value = simpleMapping.Unk1;
            Unk2.Value = simpleMapping.Unk2;
            Unk3.Value = simpleMapping.Unk3;

            var transform = simpleMapping.AFromBTransform;
            var pos = transform.Translation;
            var rot = transform.Rotation;
            var scale = transform.Scale;

            Translation.Value = new( pos.X, pos.Y, pos.Z, pos.W );
            Rotation.Value = ParsedQuat.ToEuler( new( rot.X, rot.Y, rot.Z, rot.W ) );
            Scale.Value = new( scale.X, scale.Y, scale.Z, scale.W );
        }

        public void Draw() {
            if( ImGui.Checkbox( "Display Raw Indexes", ref Plugin.Configuration.SklbMappingIndexDisplay ) ) Plugin.Configuration.Save();

            if( Plugin.Configuration.SklbMappingIndexDisplay ) {
                BoneA.Draw();
                BoneB.Draw();
            }
            else {
                BoneA.Draw( Mapping.MappedSkeleton );
                BoneB.Draw( Mapping.Bones.Bones );
            }

            Unk1.Draw();
            Unk2.Draw();
            Unk3.Draw();
            Translation.Draw();
            Rotation.Draw();
            Scale.Draw();
        }

        public SimpleMapping ToHavok() {
            var transform = new hkQsTransformf();

            var pos = new hkVector4f {
                X = Translation.Value.X,
                Y = Translation.Value.Y,
                Z = Translation.Value.Z,
                W = Translation.Value.W
            };
            transform.Translation = pos;

            var rotation = Rotation.Quat;
            var rot = new hkQuaternionf {
                X = rotation.X,
                Y = rotation.Y,
                Z = rotation.Z,
                W = rotation.W
            };
            transform.Rotation = rot;

            var scl = new hkVector4f {
                X = Scale.Value.X,
                Y = Scale.Value.Y,
                Z = Scale.Value.Z,
                W = Scale.Value.W
            };
            transform.Scale = scl;

            var simpleMapping = new SimpleMapping() {
                BoneA = ( short )BoneA.Value,
                BoneB = ( short )BoneB.Value,
                Unk1 = Unk1.Value,
                Unk2 = Unk2.Value,
                Unk3 = Unk3.Value,
                AFromBTransform = transform
            };
            return simpleMapping;
        }
    }
}
