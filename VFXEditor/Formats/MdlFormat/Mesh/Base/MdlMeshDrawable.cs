﻿using SharpDX.Direct3D11;
using System;
using VfxEditor.DirectX;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace VfxEditor.Formats.MdlFormat.Mesh.Base {
    public abstract class MdlMeshDrawable {
        public readonly int RenderId = Renderer.NewId;

        protected Buffer Data; // starts as null

        protected uint _IndexOffset;

        public uint IndexBufferOffset => _IndexOffset;

        protected uint IndexCount;

        protected byte[] RawIndexData = Array.Empty<byte>();

        public int RawIndexDataSize => RawIndexData.Length;

        public uint GetIndexCount() => IndexCount;

        public abstract void RefreshBuffer( Device device );

        public Buffer GetBuffer( Device device ) {
            if( GetIndexCount() == 0 ) return null;
            if( Data == null ) RefreshBuffer( device );
            return Data;
        }
    }
}
