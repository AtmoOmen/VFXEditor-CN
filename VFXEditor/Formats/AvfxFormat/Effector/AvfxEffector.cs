using Dalamud.Interface.Utility.Raii;
using ImGuiNET;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Formats.AvfxFormat.Nodes;
using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxEffector : AvfxNodeWithData<EffectorType> {
        public const string NAME = "Efct";

        public readonly AvfxEnum<RotationOrder> RotationOrder = new( "旋转顺序", "RoOT" );
        public readonly AvfxEnum<CoordComputeOrder> CoordComputeOrder = new( "坐标计算顺序", "CCOT" );
        public readonly AvfxBool AffectOtherVfx = new( "影响其他视效", "bAOV" );
        public readonly AvfxBool AffectGame = new( "影响游戏", "bAGm" );
        public readonly AvfxInt LoopPointStart = new( "循环开始", "LpSt" );
        public readonly AvfxInt LoopPointEnd = new( "循环结束", "LpEd" );

        private readonly List<AvfxBase> Parsed;

        private readonly UiDisplayList Parameters;

        public AvfxEffector() : base( NAME, AvfxNodeGroupSet.EffectorColor, "EfVT" ) {
            Parsed = [
                Type,
                RotationOrder,
                CoordComputeOrder,
                AffectOtherVfx,
                AffectGame,
                LoopPointStart,
                LoopPointEnd
            ];

            Parameters = new( "参数", [
                new UiNodeGraphView( this ),
                RotationOrder,
                CoordComputeOrder,
                AffectOtherVfx,
                AffectGame,
                LoopPointStart,
                LoopPointEnd
            ] );
        }

        public override void ReadContents( BinaryReader reader, int size ) {
            Peek( reader, Parsed, size );

            ReadNested( reader, ( BinaryReader _reader, string _name, int _size ) => {
                if( _name == "Data" ) {
                    UpdateData();
                    Data?.Read( _reader, _size );
                }
            }, size );
        }

        public override void WriteContents( BinaryWriter writer ) {
            WriteNested( writer, Parsed );
            Data?.Write( writer );
        }

        protected override IEnumerable<AvfxBase> GetChildren() {
            foreach( var item in Parsed ) yield return item;
            if( Data != null ) yield return Data;
        }

        public override void UpdateData() {
            Data = Type.Value switch {
                EffectorType.PointLight => new AvfxEffectorDataPointLight(),
                EffectorType.DirectionalLight => new AvfxEffectorDataDirectionalLight(),
                EffectorType.RadialBlur => new AvfxEffectorDataRadialBlur(),
                EffectorType.BlackHole => null,
                EffectorType.CameraQuake_Variable or EffectorType.CameraQuake => new AvfxEffectorDataCameraQuake(),
                EffectorType.RadialBlur_Unknown => new AvfxEffectorDataRadialBlurUnknown(),
                EffectorType.MirrorBlur => new AvfxEffectorMirrorBlur(),
                _ => null
            };
            Data?.SetAssigned( true );
        }

        public override void Draw() {
            using var _ = ImRaii.PushId( "Effector" );
            DrawRename();
            Type.Draw();
            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );

            using var tabBar = ImRaii.TabBar( "栏", ImGuiTabBarFlags.NoCloseWithMiddleMouseButton );
            if( !tabBar ) return;

            using( var tab = ImRaii.TabItem( "参数" ) ) {
                if( tab ) Parameters.Draw();
            }

            DrawData();
        }

        private void DrawData() {
            if( Data == null ) return;

            using var tabItem = ImRaii.TabItem( "数据" );
            if( !tabItem ) return;

            Data.Draw();
        }

        public override string GetDefaultText() => $"效果器 {GetIdx()} ({Type.Value})";

        public override string GetWorkspaceId() => $"Effct{GetIdx()}";
    }
}
