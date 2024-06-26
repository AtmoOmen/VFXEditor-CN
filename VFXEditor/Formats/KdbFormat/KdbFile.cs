using Dalamud.Interface;
using Dalamud.Interface.Utility.Raii;
using ImGuiNET;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VfxEditor.FileManager;
using VfxEditor.Formats.KdbFormat.Nodes;
using VfxEditor.Formats.KdbFormat.Nodes.Types;
using VfxEditor.Interop.Havok;
using VfxEditor.Interop.Havok.Ui;
using VfxEditor.Parsing;
using VfxEditor.Parsing.Int;
using VfxEditor.Utils;

namespace VfxEditor.Formats.KdbFormat {
    public enum ArrayType : ushort {
        Operations, // size = 0x10
        B, // size = 0x10
        C, // size = 0x08
        Unused, // ???
        E, // size = 0x08
        F, // size = 0x08, actually has values
    }

    public unsafe class KdbFile : FileManagerFile {
        public static string SklbTempPath => Path.Combine( Plugin.RootLocation, "Files", "kb_havok.hkx" );
        private readonly SkeletonSelector Selector;
        public readonly List<string> BoneList = [];

        private readonly uint MajorVersion;
        private readonly uint MinorVersion;
        private readonly uint PatchVersion;
        private readonly uint CheckFileSize;

        private readonly ParsedFnvHash FileName = new( "File Name" );

        private readonly ParsedDouble UnknownOperation = new( "Unknown Operation" );
        private readonly ParsedUInt UnknownB1 = new( "Unknown B1" );
        private readonly ParsedUInt UnknownB2 = new( "Unknown B2" );

        public readonly KdbNodeGraphViewer NodeGraph = new();

        public KdbFile( BinaryReader reader, string sourcePath, bool verify ) : base() {
            Selector = new( GetSklbPath( sourcePath ), UpdateSkeleton );

            // ==========================

            MajorVersion = reader.ReadUInt32();
            MinorVersion = reader.ReadUInt32();
            PatchVersion = reader.ReadUInt32();
            CheckFileSize = reader.ReadUInt32();
            reader.ReadUInt32(); // file size

            FileName.Read( reader, sourcePath.Split( '\\' )[^1].Split( '.' )[0] ); // encoded like kdi_c0101t0778
            reader.ReadUInt16(); // name offset, 0
            reader.ReadUInt16(); // padding

            var dataArrayCount = reader.ReadUInt32();
            if( dataArrayCount != 5 ) Dalamud.Error( $"Data array count is {dataArrayCount}" );
            var dataArrayPosition = reader.BaseStream.Position + reader.ReadUInt32();

            for( var i = 0; i < 7; i++ ) reader.ReadUInt32(); // reserved

            reader.BaseStream.Position = dataArrayPosition;
            var dataArrayPositions = new List<(ArrayType, long)>();
            for( var i = 0; i < dataArrayCount; i++ ) {
                var type = ( ArrayType )reader.ReadUInt16();
                var unknown = reader.ReadUInt16();
                if( unknown != 1 ) Dalamud.Error( $"Value is {unknown}" );
                dataArrayPositions.Add( (type, reader.BaseStream.Position + reader.ReadUInt32()) ); // add offset
            }

            foreach( var (type, position) in dataArrayPositions ) {
                reader.BaseStream.Position = position;
                var count = reader.ReadUInt32();
                var arrayPosition = reader.BaseStream.Position + reader.ReadUInt32(); // offset is 0 if count = 0

                if( type == ArrayType.Operations ) {
                    UnknownOperation.Read( reader );
                    reader.BaseStream.Position = arrayPosition;

                    var nodes = new List<KdbNode>();
                    for( var nodeIdx = 0; nodeIdx < count; nodeIdx++ ) {
                        reader.ReadUInt32(); // index
                        var nodeType = ( KdbNodeType )reader.ReadByte();

                        KdbNode newNode = nodeType switch {
                            KdbNodeType.EffectorExpr => new KdbNodeEffectorExpr( reader ),
                            KdbNodeType.EffectorEZParamLink => new KdbNodeEffectorEZParamLink( reader ),
                            KdbNodeType.EffectorEZParamLinkLinear => new KdbNodeEffectorEZParamLinkLinear( reader ),
                            KdbNodeType.SourceOther => new KdbNodeSourceOther( reader ),
                            KdbNodeType.SourceRotate => new KdbNodeSourceRotate( reader ),
                            KdbNodeType.SourceTranslate => new KdbNodeSourceTranslate( reader ),
                            KdbNodeType.TargetBendSTRoll => new KdbNodeTargetBendSTRoll( reader ),
                            KdbNodeType.TargetRotate => new KdbNodeTargetRotate( reader ),
                            KdbNodeType.TargetTranslate => new KdbNodeTargetTranslate( reader ),
                            KdbNodeType.TargetBendRoll => new KdbNodeTargetBendRoll( reader ),
                            KdbNodeType.TargetOrientationConstraint => new KdbNodeTargetOrientationConstraint( reader ),
                            KdbNodeType.TargetPosContraint => new KdbTargetPosConstraint( reader ),
                            KdbNodeType.TargetScale => new KdbNodeTargetScale( reader ),
                            // ========================
                            KdbNodeType.Connection => new KdbConnection( reader ),
                            _ => null
                        };

                        if( newNode == null ) Dalamud.Error( $"Unknown node type {nodeType}" );
                        else nodes.Add( newNode );
                    }

                    foreach( var node in nodes.Where( x => x is not KdbConnection ) ) NodeGraph.AddToCanvas( node, false );
                    foreach( var node in nodes ) {
                        if( node is not KdbConnection connection ) continue;

                        var sourceNode = nodes[connection.SourceIdx];
                        var targetNode = nodes[connection.TargetIdx];
                        var sourceSlot = sourceNode.FindOutput( connection.SourceType );
                        var targetSlot = targetNode.FindInput( connection.TargetType );
                        if( sourceSlot == null ) {
                            Dalamud.Error( $"Could not find output {connection.SourceType} for {sourceNode.Type}" );
                            continue;
                        }
                        if( targetSlot == null ) {
                            Dalamud.Error( $"Could not find input {connection.TargetType} for {targetNode.Type}" );
                            continue;
                        }
                        if( !targetSlot.AcceptMultiple && targetSlot.GetConnections().Count > 0 ) {
                            Dalamud.Error( $"{connection.TargetType} for {targetNode.Type} should accept multiple inputs" );
                        }
                        targetSlot.Connect( sourceSlot, connection.Coeff, connection.Unknown );
                    }

                    NodeGraph.Canvas.Organize();
                }
                else if( type == ArrayType.B ) {
                    UnknownB1.Read( reader );
                    UnknownB2.Read( reader );
                    reader.BaseStream.Position = arrayPosition;

                    // TODO
                }
            }
        }

        public unsafe void UpdateSkeleton( SimpleSklb sklbFile ) {
            sklbFile.SaveHavokData( SklbTempPath );
            var bones = new HavokBones( SklbTempPath, true );
            BoneList.Clear();
            var skeleton = bones.AnimationContainer->Skeletons[0].ptr;
            for( var i = 0; i < skeleton->Bones.Length; i++ ) BoneList.Add( skeleton->Bones[i].Name.String );
            foreach( var node in NodeGraph.Canvas.Nodes ) node.UpdateBones( BoneList );
            bones.RemoveReference();
        }

        public override void Write( BinaryWriter writer ) {

        }

        public override void Draw() {
            ImGui.Separator();

            ImGui.TextDisabled( $"Version: {MajorVersion}.{MinorVersion}.{PatchVersion}" );
            FileName.Draw();
            UnknownOperation.Draw();
            UnknownB1.Draw();
            UnknownB2.Draw();

            ImGui.Separator();
            Selector.Draw();
            ImGui.Separator();

            using( var graphChild = ImRaii.Child( "GraphChild", new( -1, ImGui.GetContentRegionAvail().Y / 2f ) ) ) {
                NodeGraph.Draw();
            }

            using var selectedChild = ImRaii.Child( "SelectedChild" );
            if( NodeGraph.Canvas.SelectedNodes.Count == 0 ) return;

            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 10 );

            var node = NodeGraph.Canvas.SelectedNodes.FirstOrDefault();
            using( var font = ImRaii.PushFont( UiBuilder.IconFont ) ) {
                if( UiUtils.RemoveButton( FontAwesomeIcon.Trash.ToIconString() ) ) NodeGraph.Canvas.RemoveNode( node );
            }
            node.Draw();
        }

        private static string GetSklbPath( string sourcePath ) {
            // chara/human/c0101/skeleton/met/m0005/skl_c0101m0005.sklb
            // chara/human/c0101/skeleton/top/t6188/skl_c0101t6188.sklb
            // chara/monster/m0011/skeleton/base/b0001/skl_m0011b0001.sklb
            // chara/demihuman/d1002/skeleton/base/b0001/skl_d1002b0001.sklb
            // chara/human/c0401/skeleton/base/b0001/skl_c0401b0001.sklb
            // chara/human/c0401/skeleton/hair/h0004/skl_c0401h0004.sklb
            // chara/human/c0401/skeleton/face/f0202/skl_c0401f0202.sklb

            var combined = sourcePath.Split( "_" )[^1].Split( "." )[0]; // .../kdi_c1701f0002.kdb 0> c1701f0002
            var part1 = combined[..5]; // c1701
            var part2 = combined.Substring( 5, 5 ); // f0002
            var type1 = part1[0]; // c
            var type2 = part2[0]; // f

            var string1 = type1 switch {
                'd' => "demihuman",
                'm' => "monster",
                _ => "human"
            };

            var string2 = type2 switch {
                'f' => "face",
                'h' => "hair",
                't' => "top",
                'm' => "met",
                _ => "base'"
            };

            return $"chara/{string1}/{part1}/skeleton/{string2}/{part2}/skl_{combined}.sklb";
        }
    }
}
