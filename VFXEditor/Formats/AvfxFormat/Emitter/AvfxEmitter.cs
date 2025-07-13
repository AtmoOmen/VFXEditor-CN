using Dalamud.Interface;
using Dalamud.Interface.Utility.Raii;
using ImGuiNET;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Formats.AvfxFormat.Nodes;
using VfxEditor.Ui.Interfaces;
using VfxEditor.Utils;
using VFXEditor.Formats.AvfxFormat.Curve;
using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxEmitter : AvfxNodeWithData<EmitterType> {
        public const string NAME = "Emit";

        public readonly AvfxString Sound = new( "音效", "SdNm", true, false, null, true );
        public readonly AvfxInt SoundNumber = new( "音频索引", "SdNo" );
        public readonly AvfxInt LoopStart = new( "循环开始", "LpSt" );
        public readonly AvfxInt LoopEnd = new( "循环结束", "LpEd" );
        public readonly AvfxInt ChildLimit = new( "子级限制", "ClCn" );
        public readonly AvfxInt EffectorIdx = new( "效果器选择", "EfNo", value: -1 );
        public readonly AvfxBool AnyDirection = new( "任意方向", "bAD", size: 1 );
        public readonly AvfxEnum<RotationDirectionBase> RotationDirectionBaseType = new( "旋转方向基准", "RBDT" );
        public readonly AvfxEnum<CoordComputeOrder> CoordComputeOrderType = new( "坐标计算顺序", "CCOT" );
        public readonly AvfxEnum<RotationOrder> RotationOrderType = new( "旋转顺序", "ROT" );
        public readonly AvfxInt ParticleCount = new( "粒子数", "PrCn" );
        public readonly AvfxInt EmitterCount = new( "发射器数", "EmCn" );
        public readonly AvfxLife Life = new();
        public readonly AvfxCurve1Axis CreateCount = new( "创建数", "CrC", locked: true );
        public readonly AvfxCurve1Axis CreateCountRandom = new( "随机创建数", "CrCR" );
        public readonly AvfxCurve1Axis CreateInterval = new( "创建数间隔", "CrI", locked: true );
        public readonly AvfxCurve1Axis CreateIntervalRandom = new( "随机创建数间隔", "CrIR" );
        public readonly AvfxCurve1Axis Gravity = new( "重力", "Gra" );
        public readonly AvfxCurve1Axis GravityRandom = new( "随机重力", "GraR" );
        public readonly AvfxCurve1Axis AirResistance = new( "空气阻力", "ARs", locked: true );
        public readonly AvfxCurve1Axis AirResistanceRandom = new( "随机空气阻力", "ARsR" );
        public readonly AvfxCurveColor Color = new( "颜色", locked: true );
        public readonly AvfxCurve3Axis Position = new( "位置", "Pos", locked: true );
        public readonly AvfxCurve3Axis Rotation = new( "旋转", "Rot", CurveType.Angle, locked: true );
        public readonly AvfxCurve3Axis Scale = new( "缩放", "Scl", locked: true );

        public readonly AvfxCurve1Axis InjectionAngleX = new( "X 轴注入角度", "IAX", CurveType.Angle );
        public readonly AvfxCurve1Axis InjectionAngleRandomX = new( "随机 X 轴注入角度", "IAXR", CurveType.Angle );

        public readonly AvfxCurve1Axis InjectionAngleY = new( "Y 轴注入角度", "IAY", CurveType.Angle );
        public readonly AvfxCurve1Axis InjectionAngleRandomY = new( "随机 Y 轴注入角度", "IAYR", CurveType.Angle );

        public readonly AvfxCurve1Axis InjectionAngleZ = new( "Z 轴注入角度", "IAZ", CurveType.Angle );
        public readonly AvfxCurve1Axis InjectionAngleRandomZ = new( "随机 Z 轴注入角度", "IAZR", CurveType.Angle );

        public readonly AvfxCurve1Axis VelocityRandomX = new( "随机 X 轴速度", "VRX" );
        public readonly AvfxCurve1Axis VelocityRandomY = new( "随机 Y 轴速度", "VRY" );
        public readonly AvfxCurve1Axis VelocityRandomZ = new( "随机 Z 轴速度", "VRZ" );


        private readonly List<AvfxBase> Parsed;

        public readonly List<AvfxEmitterItem> Particles = [];
        public readonly List<AvfxEmitterItem> Emitters = [];

        private readonly UiNodeGraphView NodeView;
        public readonly AvfxNodeGroupSet NodeGroups;

        public readonly AvfxNodeSelect<AvfxEffector> EffectorSelect;

        public readonly AvfxDisplaySplitView<AvfxItem> AnimationSplitDisplay;

        public readonly UiEmitterSplitView EmitterSplit;
        public readonly UiEmitterSplitView ParticleSplit;
        private readonly List<IUiItem> Parameters;

        public AvfxEmitter( AvfxNodeGroupSet groupSet ) : base( NAME, AvfxNodeGroupSet.EmitterColor, "EVT" ) {
            NodeGroups = groupSet;

            Parsed = [
                Sound,
                SoundNumber,
                LoopStart,
                LoopEnd,
                ChildLimit,
                EffectorIdx,
                AnyDirection,
                Type,
                RotationDirectionBaseType,
                CoordComputeOrderType,
                RotationOrderType,
                ParticleCount,
                EmitterCount,
                Life,
                CreateCount,
                CreateCountRandom,
                CreateInterval,
                CreateIntervalRandom,
                Gravity,
                GravityRandom,
                AirResistance,
                AirResistanceRandom,
                Color,
                Position,
                Rotation,
                Scale,
                InjectionAngleX,
                InjectionAngleRandomX,
                InjectionAngleY,
                InjectionAngleRandomY,
                InjectionAngleZ,
                InjectionAngleRandomZ,
                VelocityRandomX,
                VelocityRandomY,
                VelocityRandomZ
            ];
            Sound.SetAssigned( false );

            AnimationSplitDisplay = new( "Animation", [
                Life,
                CreateCount,
                CreateCountRandom,
                CreateInterval,
                CreateIntervalRandom,
                Gravity,
                GravityRandom,
                AirResistance,
                AirResistanceRandom,
                Color,
                Position,
                Rotation,
                Scale,
                InjectionAngleX,
                InjectionAngleRandomX,
                InjectionAngleY,
                InjectionAngleRandomY,
                InjectionAngleZ,
                InjectionAngleRandomZ,
                VelocityRandomX,
                VelocityRandomY,
                VelocityRandomZ
            ] );

            EffectorSelect = new( this, "效果器选择", groupSet.Effectors, EffectorIdx );
            EmitterSplit = new( "Create Emitters", Emitters, this, false );
            ParticleSplit = new( "Create Particles", Particles, this, true );

            NodeView = new UiNodeGraphView( this );

            Parameters = [
                LoopStart,
                LoopEnd,
                ChildLimit,
                AnyDirection,
                RotationDirectionBaseType,
                CoordComputeOrderType,
                RotationOrderType
            ];

            Sound.Parsed.Icons.Insert( 0, new() {
                Icon = () => FontAwesomeIcon.VolumeUp,
                Remove = false,
                Action = ( string value ) => Plugin.ResourceLoader.PlaySound( value, SoundNumber.Value )
            } );
        }

        public override void ReadContents( BinaryReader reader, int size ) {
            Peek( reader, Parsed, size );

            AvfxEmitterItemContainer lastParticle = null;
            AvfxEmitterItemContainer lastEmitter = null;

            ReadNested( reader, ( BinaryReader _reader, string _name, int _size ) => {
                if( _name == "Data" ) {
                    UpdateData();
                    Data?.Read( _reader, _size );
                }
                else if( _name == "ItPr" ) {
                    lastParticle = new AvfxEmitterItemContainer( "ItPr", true, this );
                    lastParticle.Read( _reader, _size );

                }
                else if( _name == "ItEm" ) {
                    lastEmitter = new AvfxEmitterItemContainer( "ItEm", false, this );
                    lastEmitter.Read( _reader, _size );

                }
            }, size );

            if( lastParticle != null ) {
                Particles.AddRange( lastParticle.Items );
                Particles.ForEach( x => x.InitializeNodeSelects() );
            }

            if( lastEmitter != null ) {
                var startIndex = Particles.Count;
                var emitterCount = lastEmitter.Items.Count - Particles.Count;
                Emitters.AddRange( lastEmitter.Items.GetRange( startIndex, emitterCount ) ); // remove particles
                Emitters.ForEach( x => x.InitializeNodeSelects() );
            }

            EmitterSplit.UpdateIdx();
            ParticleSplit.UpdateIdx();
        }

        public override void WriteContents( BinaryWriter writer ) {
            EmitterCount.Value = Emitters.Count;
            ParticleCount.Value = Particles.Count;
            WriteNested( writer, Parsed );

            // ItPr
            for( var i = 0; i < Particles.Count; i++ ) {
                var ItPr = new AvfxEmitterItemContainer( "ItPr", true, this );
                ItPr.Items.AddRange( Particles.GetRange( 0, i + 1 ) );
                ItPr.Write( writer );
            }

            // ItEm
            for( var i = 0; i < Emitters.Count; i++ ) {
                var ItEm = new AvfxEmitterItemContainer( "ItEm", false, this );
                ItEm.Items.AddRange( Particles );
                ItEm.Items.AddRange( Emitters.GetRange( 0, i + 1 ) );
                ItEm.Write( writer );
            }

            Data?.Write( writer );
        }

        protected override IEnumerable<AvfxBase> GetChildren() {
            foreach( var item in Parsed ) yield return item;
            if( Data != null ) yield return Data;
        }

        public override void UpdateData() {
            Data = Type.Value switch {
                EmitterType.Point => null,
                EmitterType.Cone => new AvfxEmitterDataCone(),
                EmitterType.ConeModel => new AvfxEmitterDataConeModel(),
                EmitterType.SphereModel => new AvfxEmitterDataSphereModel(),
                EmitterType.CylinderModel => new AvfxEmitterDataCylinderModel(),
                EmitterType.Model => new AvfxEmitterDataModel( this ),
                _ => null,
            };
            Data?.SetAssigned( true );
        }

        public override void Draw() {
            using var _ = ImRaii.PushId( "Emitter" );
            DrawRename();
            Type.Draw();
            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );

            using var tabBar = ImRaii.TabBar( "栏", ImGuiTabBarFlags.NoCloseWithMiddleMouseButton );
            if( !tabBar ) return;

            DrawParameters();
            DrawData();

            using( var tab = ImRaii.TabItem( "动画" ) ) {
                if( tab ) AnimationSplitDisplay.Draw();
            }

            using( var tab = ImRaii.TabItem( "创建粒子" ) ) {
                if( tab ) ParticleSplit.Draw();
            }

            using( var tab = ImRaii.TabItem( "创建发射器" ) ) {
                if( tab ) EmitterSplit.Draw();
            }
        }

        private void DrawParameters() {
            using var tabItem = ImRaii.TabItem( "参数" );
            if( !tabItem ) return;

            using var _ = ImRaii.PushId( "Parameters" );
            using var child = ImRaii.Child( "Child" );

            NodeView.Draw();
            EffectorSelect.Draw();

            Sound.Draw();
            SoundNumber.Draw();
            ImGui.SameLine();
            UiUtils.HelpMarker( "-1 if no sound" );

            DrawItems( Parameters );
        }

        private void DrawData() {
            if( Data == null ) return;

            using var tabItem = ImRaii.TabItem( "数据" );
            if( !tabItem ) return;

            Data.Draw();
        }

        public override string GetDefaultText() => $"Emitter {GetIdx()} ({Type.Value})";

        public override string GetWorkspaceId() => $"Emit{GetIdx()}";

        public override void GetChildrenRename( Dictionary<string, string> renameDict ) {
            Emitters.ForEach( item => IWorkspaceUiItem.GetRenamingMap( item, renameDict ) );
            Particles.ForEach( item => IWorkspaceUiItem.GetRenamingMap( item, renameDict ) );
        }

        public override void SetChildrenRename( Dictionary<string, string> renameDict ) {
            Emitters.ForEach( item => IWorkspaceUiItem.ReadRenamingMap( item, renameDict ) );
            Particles.ForEach( item => IWorkspaceUiItem.ReadRenamingMap( item, renameDict ) );
        }
    }
}
