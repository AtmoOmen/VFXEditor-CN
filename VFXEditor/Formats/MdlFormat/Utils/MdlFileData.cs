﻿using System.Collections.Generic;
using VfxEditor.Formats.MdlFormat.Mesh;
using VfxEditor.Formats.MdlFormat.Mesh.Shape;
using VfxEditor.Formats.MdlFormat.Mesh.TerrainShadow;

namespace VfxEditor.Formats.MdlFormat.Utils {
    public class MdlFileData {
        public readonly List<uint> VertexBufferSizes = [];
        public readonly List<uint> IndexBufferSizes = [];
        public readonly List<uint> VertexBufferOffsets = [];
        public readonly List<uint> IndexBufferOffsets = [];

        public readonly List<MdlMesh> Meshes = [];
        public readonly List<MdlTerrainShadowMesh> TerrainShadowMeshes = [];
        public readonly List<MdlSubMesh> SubMeshes = [];
        public readonly List<MdlTerrainShadowSubmesh> TerrainShadowSubmeshes = [];

        public readonly Dictionary<uint, string> OffsetToString = [];
        public MdlStringTable StringTable = new();

        public readonly List<ushort> SubmeshBoneMap = [];

        public readonly List<MdlShape> Shapes = [];
        public readonly List<MdlShapeMesh> ShapesMeshes = [];
        public readonly List<MdlShapeValue> ShapeValues = [];

        public readonly List<Queue<uint>> IndexBufferPositions = [new(), new(), new()];
    }
}
