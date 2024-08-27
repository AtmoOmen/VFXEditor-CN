using ImGuiNET;
using System;
using System.Numerics;
using VfxEditor.DirectX.Renderers;

namespace VfxEditor.DirectX.Lights {
    [Serializable]
    public class LightConfiguration {
        public Vector3 Position;
        public Vector3 Color;
        public float Radius;
        public float Falloff;

        public LightConfiguration( Vector3 position, Vector3 color, float radius, float falloff ) {
            Position = position;
            Color = color;
            Radius = radius;
            Falloff = falloff;
        }

        public LightData GetData() => new() {
            Position = DirectXManager.ToVec3( Position ),
            Color = DirectXManager.ToVec3( Color ),
            Radius = Radius,
            Falloff = Falloff
        };

        public bool Draw() {
            var updated = false;

            updated |= ImGui.InputFloat3( "光照位置", ref Position );
            updated |= ImGui.ColorEdit3( "光照颜色", ref Color );
            updated |= ImGui.InputFloat( "光照半径", ref Radius );
            updated |= ImGui.InputFloat( "光照衰减", ref Falloff );

            return updated;
        }
    }
}
