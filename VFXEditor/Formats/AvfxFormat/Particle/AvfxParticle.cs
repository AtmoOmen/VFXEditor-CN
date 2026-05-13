using Dalamud.Interface.Utility.Raii;
using Dalamud.Bindings.ImGui;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Formats.AvfxFormat.Nodes;
using VFXEditor.Formats.AvfxFormat.Curve;
using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxParticle : AvfxNodeWithData<ParticleType> {
        public const string NAME = "Ptcl";

        public readonly AvfxInt LoopStart = new( "循环开始", "LpSt" );
        public readonly AvfxInt LoopEnd = new( "循环结束", "LpEd" );
        public readonly AvfxEnum<RotationDirectionBase> RotationDirectionBaseType = new( "旋转方向基准", "RBDT" );
        public readonly AvfxEnum<RotationOrder> RotationOrderType = new( "旋转计算顺序", "RoOT" );
        public readonly AvfxEnum<CoordComputeOrder> CoordComputeOrderType = new( "坐标计算顺序", "CCOT" );
        public readonly AvfxEnum<DrawMode> DrawModeType = new( "绘制模式", "RMT" );
        public readonly AvfxEnum<CullingType> CullingTypeType = new( "剔除类型", "CulT" );
        public readonly AvfxEnum<EnvLight> EnvLightType = new( "环境光", "EnvT" );
        public readonly AvfxEnum<DirLight> DirLightType = new( "定向光", "DirT" );
        public readonly AvfxEnum<UvPrecision> UvPrecisionType = new( "平面坐标精度", "UVPT" );
        public readonly AvfxInt DrawPriority = new( "绘制优先级", "DwPr" );
        public readonly AvfxBool IsDepthTest = new( "深度测试", "DsDt" );
        public readonly AvfxBool IsDepthWrite = new( "深度写入", "DsDw" );
        public readonly AvfxBool IsSoftParticle = new( "柔性粒子", "DsSp" );
        public readonly AvfxInt CollisionType = new( "碰撞类型", "Coll" );
        public readonly AvfxBool S11Enabled = new( "启用 S11", "bS11" );

        // New to dawntrail
        public readonly AvfxInt ShUT = new( "ShUT", "ShUT" );
        public readonly AvfxInt ShR = new( "ShR", "ShR" );
        public readonly AvfxInt ShT = new( "ShT", "ShT" );
        public readonly AvfxInt UniV = new( "统一速度", "UniV" );
        public readonly AvfxInt HybV = new( "混合速度", "HybV" );
        public readonly AvfxBool E24Enabled = new( "启用 E24", "bE24" );

        public readonly AvfxBool IsApplyToneMap = new( "应用色调映射", "bATM" );
        public readonly AvfxBool IsApplyFog = new( "应用雾效", "bAFg" );
        public readonly AvfxBool ClipNearEnable = new( "启用近裁剪", "bNea" );
        public readonly AvfxBool ClipFarEnable = new( "启用远裁剪", "bFar" );
        public readonly AvfxFloat ClipNearStart = new( "近裁剪开始位置", "NeSt" ); // float2
        public readonly AvfxFloat ClipNearEnd = new( "近裁剪结束位置", "NeEd" );
        public readonly AvfxFloat ClipFarStart = new( "远裁剪开始位置", "FaSt" ); // float2
        public readonly AvfxFloat ClipFarEnd = new( "远裁剪结束位置", "FaEd" );
        public readonly AvfxEnum<ClipBasePoint> ClipBasePointType = new( "裁剪基准点", "FaBP" );
        public readonly AvfxInt UvSetCount = new( "平面坐标集数量", "UvSN" );
        public readonly AvfxInt ApplyRateEnvironment = new( "应用环境光强度", "EvAR" );
        public readonly AvfxInt ApplyRateDirectional = new( "应用定向光强度", "DlAR" );
        public readonly AvfxInt ApplyRateLightBuffer = new( "应用光缓冲率", "LBAR" );
        public readonly AvfxEnum<DepthOffsetType> DOTy = new( "深度偏移类型", "DOTy" );
        public readonly AvfxFloat DepthOffset = new( "深度偏移", "DpOf" );
        public readonly AvfxBool SimpleAnimEnable = new( "使用简易动画", "bSCt" );
        public readonly AvfxLife Life = new();
        public readonly AvfxCurve1Axis Gravity = new( "Gravity", "Gra" );
        public readonly AvfxCurve1Axis GravityRandom = new( "Gravity Random", "GraR" );
        public readonly AvfxCurve1Axis AirResistance = new( "Air Resistance", "ARs", locked: true );
        public readonly AvfxCurve1Axis AirResistanceRandom = new( "Air Resistance Random", "ARsR", locked: true );
        public readonly AvfxCurve3Axis Scale = new( "Scale", "Scl", locked: true );
        public readonly AvfxCurve3Axis Rotation = new( "Rotation", "Rot", CurveType.Angle, locked: true );
        public readonly AvfxCurve3Axis Position = new( "Position", "Pos", locked: true );
        public readonly AvfxCurve1Axis RotVelX = new( "Rotation Velocity X", "VRX" );
        public readonly AvfxCurve1Axis RotVelXRandom = new( "Rotation Velocity X Random", "VRXR" );
        public readonly AvfxCurve1Axis RotVelY = new( "Rotation Velocity Y", "VRY" );
        public readonly AvfxCurve1Axis RotVelYRandom = new( "Rotation Velocity Y Random", "VRYR" );
        public readonly AvfxCurve1Axis RotVelZ = new( "Rotation Velocity Z", "VRZ" );
        public readonly AvfxCurve1Axis RotVelZRandom = new( "Rotation Velocity Z Random", "VRZR" );
        public readonly AvfxCurveColor Color;

        // initialize these later
        public readonly AvfxParticleTextureColor1 TC1;
        public readonly AvfxParticleTextureColor2 TC2;
        public readonly AvfxParticleTextureColor2 TC3;
        public readonly AvfxParticleTextureColor2 TC4;
        public readonly AvfxParticleTextureNormal TN;
        public readonly AvfxParticleTextureReflection TR;
        public readonly AvfxParticleTextureDistortion TD;
        public readonly AvfxParticleTexturePalette TP;
        public readonly AvfxParticleSimple Simple;

        public readonly List<AvfxParticleUvSet> UvSets = [];
        public readonly UiUvSetSplitView UvView;

        private readonly List<AvfxBase> Parsed;
        private readonly List<AvfxBase> Parsed2;

        public readonly AvfxNodeGroupSet NodeGroups;
        public readonly AvfxDisplaySplitView<AvfxItem> AnimationSplitDisplay;
        public readonly AvfxDisplaySplitView<AvfxItem> TextureDisplaySplit;
        private readonly UiDisplayList Parameters;

        public AvfxParticle( AvfxFile file, AvfxNodeGroupSet groupSet ) : base( file, NAME, AvfxNodeGroupSet.ParticleColor, "PrVT" ) {
            NodeGroups = groupSet;

            // Initialize the remaining ones

            TC1 = new AvfxParticleTextureColor1( this );
            TC2 = new AvfxParticleTextureColor2( "材质颜色 2", "TC2", this );
            TC3 = new AvfxParticleTextureColor2( "材质颜色 3", "TC3", this );
            TC4 = new AvfxParticleTextureColor2( "材质颜色 4", "TC4", this );
            TN = new AvfxParticleTextureNormal( this );
            TR = new AvfxParticleTextureReflection( this );
            TD = new AvfxParticleTextureDistortion( this );
            TP = new AvfxParticleTexturePalette( this );
            Simple = new AvfxParticleSimple( this );
            Color = new( File, "Color", locked: true );

            // Parsing

            Parsed = [
                LoopStart,
                LoopEnd,
                Type,
                RotationDirectionBaseType,
                RotationOrderType,
                CoordComputeOrderType,
                DrawModeType,
                CullingTypeType,
                EnvLightType,
                DirLightType,
                UvPrecisionType,
                DrawPriority,
                IsDepthTest,
                IsDepthWrite,
                IsSoftParticle,
                CollisionType,
                S11Enabled,
                ShUT,
                ShR,
                ShT,
                UniV,
                HybV,
                E24Enabled,
                IsApplyToneMap,
                IsApplyFog,
                ClipNearEnable,
                ClipFarEnable,
                ClipNearStart,
                ClipNearEnd,
                ClipFarStart,
                ClipFarEnd,
                ClipBasePointType,
                UvSetCount,
                ApplyRateEnvironment,
                ApplyRateDirectional,
                ApplyRateLightBuffer,
                DOTy,
                DepthOffset,
                SimpleAnimEnable,
                Life,
                Simple,
                Gravity,
                GravityRandom,
                AirResistance,
                AirResistanceRandom,
                Scale,
                Rotation,
                Position,
                RotVelX,
                RotVelXRandom,
                RotVelY,
                RotVelYRandom,
                RotVelZ,
                RotVelZRandom,
                Color
            ];

            Parsed2 = [
                TC1,
                TC2,
                TC3,
                TC4,
                TN,
                TR,
                TD,
                TP
            ];
            
            // Drawing

            Parameters = new( "Parameters", [
                new UiNodeGraphView( file, this ),
                LoopStart,
                LoopEnd,
                SimpleAnimEnable,
                RotationDirectionBaseType,
                RotationOrderType,
                CoordComputeOrderType,
                DrawModeType,
                CullingTypeType,
                EnvLightType,
                DirLightType,
                UvPrecisionType,
                DrawPriority,
                IsDepthTest,
                IsDepthWrite,
                IsSoftParticle,
                CollisionType,
                S11Enabled,
                ShT,
                UniV,
                HybV,
                ShUT,
                ShR,
                E24Enabled,
                IsApplyToneMap,
                IsApplyFog,
                ClipNearEnable,
                ClipFarEnable,
                new UiFloat2( "近裁剪", ClipNearStart, ClipNearEnd ),
                new UiFloat2( "远裁剪", ClipFarStart, ClipFarEnd ),
                ClipBasePointType,
                ApplyRateEnvironment,
                ApplyRateDirectional,
                ApplyRateLightBuffer,
                DOTy,
                DepthOffset,
            ] );

            AnimationSplitDisplay = new( "动画", [
                new UiDisplayList( "动画", [
                    Gravity,
                    GravityRandom,
                    AirResistance,
                    AirResistanceRandom,
                    Scale,
                    Rotation,
                    Position,
                    RotVelX,
                    RotVelY,
                    RotVelZ,
                    RotVelXRandom,
                    RotVelYRandom,
                    RotVelZRandom
                ] ),
                new UiDisplayList( "颜色", [
                    Color
                ] )
            ] );
            
            UvView = new( UvSets );

            TextureDisplaySplit = new( "材质", [
                TC1,
                TC2,
                TC3,
                TC4,
                TN,
                TR,
                TD,
                TP,
            ] );
        }

        public override void ReadContents( BinaryReader reader, int size ) {
            Peek( reader, Parsed,  size );
            Peek( reader, Parsed2, size );

            UpdateData(); // TODO: check if moving this here breaks anything

            ReadNested( reader, ( _reader, _name, _size ) => {
                if( _name == "Data" ) {
                    Data?.Read( _reader, _size );
                }
                else if( _name == "UvSt" ) {
                    var uvSet = new AvfxParticleUvSet();
                    uvSet.Read( _reader, _size );
                    UvSets.Add( uvSet );
                }
            }, size );

            UvView.UpdateIdx();
        }

        public override void WriteContents( BinaryWriter writer ) {
            UvSetCount.Value = UvSets.Count;
            WriteNested( writer, Parsed );

            foreach( var uvSet in UvSets ) uvSet.Write( writer );

            Data?.Write( writer );
            WriteNested( writer, Parsed2 );
        }

        protected override IEnumerable<AvfxBase> GetChildren() {
            foreach( var item in Parsed ) yield return item;
            if( Data != null ) yield return Data;
            foreach( var item in Parsed2 ) yield return item;
        }

        public override void UpdateData() {
            Data = Type.Value switch {
                ParticleType.Parameter => null,
                ParticleType.Powder => new AvfxParticleDataPowder(),
                ParticleType.Windmill => new AvfxParticleDataWindmill(),
                ParticleType.Line => new AvfxParticleDataLine( File ),
                ParticleType.Model => new AvfxParticleDataModel( File, this ),
                ParticleType.Polyline => new AvfxParticleDataPolyline( File ),
                ParticleType.Quad => new AvfxParticleDataQuad(),
                ParticleType.Polygon => new AvfxParticleDataPolygon(),
                ParticleType.Decal => new AvfxParticleDataDecal(),
                ParticleType.DecalRing => new AvfxParticleDataDecalRing(),
                ParticleType.Disc => new AvfxParticleDataDisc( File ),
                ParticleType.LightModel => new AvfxParticleDataLightModel( this ),
                ParticleType.Laser => new AvfxParticleDataLaser(),
                ParticleType.ModelSkin => new AvfxParticleDataModelSkin( File ),
                ParticleType.Dissolve => new AvfxParticleDataDissolve( File ),
                _ => null,
            };

            Data?.SetAssigned( !Data.Optional );
        }

        public override void Draw() {
            using var _ = ImRaii.PushId( "Particle" );
            DrawRename();
            Type.Draw();
            Data?.DrawEnableCheckbox();
            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );

            using var tabBar = ImRaii.TabBar( "Tabs", ImGuiTabBarFlags.NoCloseWithMiddleMouseButton );
            if( !tabBar ) return;

            using( var tab = ImRaii.TabItem( "参数" ) ) {
                if( tab ) Parameters.Draw();
            }

            DrawData();

            using( var tab = ImRaii.TabItem( "动画" ) ) {
                if( tab ) AnimationSplitDisplay.Draw();
            }

            using( var tab = ImRaii.TabItem( "UV 集" ) ) {
                if( tab ) UvView.Draw();
            }

            using( var tab = ImRaii.TabItem( "材质" ) ) {
                if( tab ) TextureDisplaySplit.Draw();
            }
        }

        private void DrawData() {
            if( Data == null || ( Data.Optional && !Data.IsAssigned() ) ) return;

            using var tabItem = ImRaii.TabItem( "Data" );
            if( !tabItem ) return;

            Data.Draw();
        }

        public override string GetDefaultText() => $"粒子 {GetIdx()} ({Type.Value})";

        public override string GetWorkspaceId() => $"Ptcl{GetIdx()}";
    }
}
