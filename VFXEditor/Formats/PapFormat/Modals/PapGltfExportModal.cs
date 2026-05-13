using Dalamud.Bindings.ImGui;
using System.Collections.Generic;
using VfxEditor.PapFormat.Motion;
using VfxEditor.Ui.Components;
using VfxEditor.Utils.Gltf;

namespace VfxEditor.PapFormat {
    public unsafe class PapGltfExportModal : Modal {
        private readonly PapMotion Motion;
        private readonly string AnimationName;
        private readonly string ExportPath;

        private bool SkipUnanimated = true;

        public PapGltfExportModal( PapMotion motion, string animationName, string exportPath ) : base( "动画导出", true ) {
            Motion = motion;
            AnimationName = animationName;
            ExportPath = exportPath;
        }

        protected override void DrawBody() {
            ImGui.Checkbox( "跳过未绑定动画的骨骼", ref SkipUnanimated );
        }

        protected override void OnCancel() { }

        protected override void OnOk() {
            GltfAnimation.ExportAnimation(
                Motion.File.MotionData.Skeleton,
                new List<string>( [AnimationName] ),
                new List<PapMotion>( [Motion] ),
                SkipUnanimated,
                ExportPath
            );
            Dalamud.OkNotification( "Havok 数据已导出" );
        }
    }
}
