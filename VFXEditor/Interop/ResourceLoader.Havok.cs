﻿using System;
using VfxEditor.Interop.Havok.Structs;

namespace VfxEditor.Interop {
    public unsafe partial class ResourceLoader {
        public static IntPtr HavokInterleavedAnimationVtbl { get; private set; }

        public static IntPtr HavokMapperVtbl { get; private set; }

        public delegate HkaSplineCompressedAnimation* HavokSplineCtorDelegate(
            HkaSplineCompressedAnimation* spline,
            HkaInterleavedUncompressedAnimation* interleaved );

        public HavokSplineCtorDelegate HavokSplineCtor { get; private set; }
    }
}
