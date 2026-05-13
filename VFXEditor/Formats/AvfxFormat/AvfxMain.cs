using Dalamud.Interface.Utility.Raii;
using Dalamud.Bindings.ImGui;
using System;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Ui.Interfaces;
using VfxEditor.Utils;
using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxMain : AvfxDrawable {
        public static AvfxMain FromStream( AvfxFile file, BinaryReader reader ) {
            var main = new AvfxMain( file );
            reader.ReadInt32();
            var size = reader.ReadInt32();
            main.Read( reader, size );
            return main;
        }

        // ============

        public readonly AvfxFile File;

        public readonly AvfxInt Version = new( "Version", "Ver", value: 0x20110913 );
        public readonly AvfxBool IsDelayFastParticle = new( "Delay Fast Particle", "bDFP" );
        public readonly AvfxBool IsFitGround = new( "Fit Ground", "bFG" );
        public readonly AvfxBool IsTranformSkip = new( "Transform Skip", "bTS" );
        public readonly AvfxBool IsAllStopOnHide = new( "All Stop on Hide", "bASH" );
        public readonly AvfxBool CanBeClippedOut = new( "Can be Clipped Out", "bCBC" );
        public readonly AvfxBool ClipBoxenabled = new( "Clip Box Enabled", "bCul" );
        public readonly AvfxFloat ClipBoxX = new( "Clip Box X", "CBPx" );
        public readonly AvfxFloat ClipBoxY = new( "Clip Box Y", "CBPy" );
        public readonly AvfxFloat ClipBoxZ = new( "Clip Box Z", "CBPz" );
        public readonly AvfxFloat ClipBoxSizeX = new( "Clip Box Size X", "CBSx" );
        public readonly AvfxFloat ClipBoxSizeY = new( "Clip Box Size Y", "CBSy" );
        public readonly AvfxFloat ClipBoxSizeZ = new( "Clip Box Size Z", "CBSz" );
        public readonly AvfxFloat BiasZmaxScale = new( "Bias Z Max Scale", "ZBMs" );
        public readonly AvfxFloat BiasZmaxDistance = new( "Bias Z Max Distance", "ZBMd" );
        public readonly AvfxBool IsCameraSpace = new( "Camera Space", "bCmS" );
        public readonly AvfxBool IsFullEnvLight = new( "Full Env Light", "bFEL" );
        public readonly AvfxBool OSE = new( "OSE", "bOSE" );
        public readonly AvfxBool IsClipOwnSetting = new( "裁剪自身", "bOSt" );
        public readonly AvfxFloat NearClipBegin = new( "近裁剪起始位置", "NCB" );
        public readonly AvfxFloat NearClipEnd = new( "近裁剪结束位置", "NCE" );
        public readonly AvfxFloat FarClipBegin = new( "远裁剪起始位置", "FCB" );
        public readonly AvfxFloat FarClipEnd = new( "远裁剪结束位置", "FCE" );
        public readonly AvfxFloat SoftParticleFadeRange = new( "柔性粒子淡化范围", "SPFR" );
        public readonly AvfxFloat SoftKeyOffset = new( "排序键偏移值", "SKO" );
        public readonly AvfxEnum<DrawLayer> DrawLayerType = new( "绘制层级", "DwLy" );
        public readonly AvfxEnum<DrawOrder> DrawOrderType = new( "绘制顺序", "DwOT" );
        public readonly AvfxEnum<DirectionalLightSource> DirectionalLightSourceType = new( "定向光源", "DLST" );
        public readonly AvfxEnum<PointLightSouce> PointLightsType1 = new( "点光源 1", "PL1S" );
        public readonly AvfxEnum<PointLightSouce> PointLightsType2 = new( "点光源 2", "PL2S" );
        public readonly AvfxFloat RevisedValuesPosX = new( "修改后 X 轴坐标", "RvPx" );
        public readonly AvfxFloat RevisedValuesPosY = new( "修改后 Y 轴坐标", "RvPy" );
        public readonly AvfxFloat RevisedValuesPosZ = new( "修改后 Z 轴坐标", "RvPz" );
        public readonly AvfxFloat RevisedValuesRotX = new( "修改后 X 轴旋转", "RvRx" );
        public readonly AvfxFloat RevisedValuesRotY = new( "修改后 Y 轴旋转", "RvRy" );
        public readonly AvfxFloat RevisedValuesRotZ = new( "修改后 Z 轴旋转", "RvRz" );
        public readonly AvfxFloat RevisedValuesScaleX = new( "修改后 X 轴缩放", "RvSx" );
        public readonly AvfxFloat RevisedValuesScaleY = new( "修改后 Y 轴缩放", "RvSy" );
        public readonly AvfxFloat RevisedValuesScaleZ = new( "修改后 Z 轴缩放", "RvSz" );
        public readonly AvfxFloat RevisedValuesR = new( "修改后红色", "RvR" );
        public readonly AvfxFloat RevisedValuesG = new( "修改后绿色", "RvG" );
        public readonly AvfxFloat RevisedValuesB = new( "修改后蓝色", "RvB" );
        public readonly AvfxBool FadeEnabledX = new( "启用 X 轴上淡化效果", "AFXe" );
        public readonly AvfxFloat FadeInnerX = new( "X 轴内部淡化", "AFXi" );
        public readonly AvfxFloat FadeOuterX = new( "X 轴外部淡化", "AFXo" );
        public readonly AvfxBool FadeEnabledY = new( "启用 Y 轴上淡化效果", "AFYe" );
        public readonly AvfxFloat FadeInnerY = new( "Y 轴内部淡化", "AFYi" );
        public readonly AvfxFloat FadeOuterY = new( "Y 轴外部淡化", "AFYo" );
        public readonly AvfxBool FadeEnabledZ = new( "启用 Z 轴上淡化效果", "AFZe" );
        public readonly AvfxFloat FadeInnerZ = new( "Z 轴内部淡化", "AFZi" );
        public readonly AvfxFloat FadeOuterZ = new( "Z 轴外部淡化", "AFZo" );
        public readonly AvfxBool GlobalFogEnabled = new( "全局烟雾效果", "bGFE" );
        public readonly AvfxFloat GlobalFogInfluence = new( "全局雾效影响", "GFIM" );
        public readonly AvfxBool LTSEnabled = new( "启用 LTS", "bLTS" );
        public readonly AvfxBool AGSEnabled = new( "启用 AGS", "bAGS" );

        // New to dawntrail
        public readonly AvfxInt APRi = new( "APri", "APri" );
        public readonly AvfxInt DPri = new( "DPri", "DPri" );
        public readonly AvfxBool SABEnabled = new( "启用 SAB", "bSAB" );
        public readonly AvfxBool SBVEnabled = new( "启用 SBV", "bSBV" );
        public readonly AvfxFloat SBVa = new( "SBVa", "SBVa" );
        public readonly AvfxBool SSVEnabled = new( "启用 SSV", "bSSV" );
        public readonly AvfxFloat SSVa = new( "SSVa", "SSVa" );
        public readonly AvfxInt SPHP = new( "SPHP", "SPHP" ); //only 0 so far

        public readonly AvfxNodeGroupSet NodeGroupSet;

        private readonly List<AvfxBase> Parsed;

        public readonly List<AvfxScheduler> Schedulers = [];
        public readonly List<AvfxTimeline> Timelines = [];
        public readonly List<AvfxEmitter> Emitters = [];
        public readonly List<AvfxParticle> Particles = [];
        public readonly List<AvfxEffector> Effectors = [];
        public readonly List<AvfxBinder> Binders = [];
        public readonly List<AvfxTexture> Textures = [];
        public readonly List<AvfxModel> Models = [];

        private readonly List<IUiItem> Display;
        private readonly int[] UiVersion = new int[4];
        private float ScaleCombined = 1.0f;

        public AvfxMain( AvfxFile file ) : base( "AVFX" ) {
            File = file;

            Parsed = [
                Version,
                IsDelayFastParticle,
                IsFitGround,
                IsTranformSkip,
                IsAllStopOnHide,
                CanBeClippedOut,
                ClipBoxenabled,
                ClipBoxX,
                ClipBoxY,
                ClipBoxZ,
                ClipBoxSizeX,
                ClipBoxSizeY,
                ClipBoxSizeZ,
                BiasZmaxScale,
                BiasZmaxDistance,
                IsCameraSpace,
                IsFullEnvLight,
                OSE,
                IsClipOwnSetting,
                NearClipBegin,
                NearClipEnd,
                FarClipBegin,
                FarClipEnd,
                SoftParticleFadeRange,
                SPHP,
                SoftKeyOffset,
                DrawLayerType,
                DrawOrderType,
                DirectionalLightSourceType,
                PointLightsType1,
                PointLightsType2,
                RevisedValuesPosX,
                RevisedValuesPosY,
                RevisedValuesPosZ,
                RevisedValuesRotX,
                RevisedValuesRotY,
                RevisedValuesRotZ,
                RevisedValuesScaleX,
                RevisedValuesScaleY,
                RevisedValuesScaleZ,
                RevisedValuesR,
                RevisedValuesG,
                RevisedValuesB,
                FadeEnabledX,
                FadeInnerX,
                FadeOuterX,
                FadeEnabledY,
                FadeInnerY,
                FadeOuterY,
                FadeEnabledZ,
                FadeInnerZ,
                FadeOuterZ,
                GlobalFogEnabled,
                GlobalFogInfluence,
                LTSEnabled,
                AGSEnabled,
                APRi,
                DPri,
                SABEnabled,
                SBVEnabled,
                SBVa,
                SSVEnabled,
                SSVa,
            ];

            NodeGroupSet = new( this );

            Display = [
                new UiFloat3( "缩放 (修改后)", RevisedValuesScaleX, RevisedValuesScaleY, RevisedValuesScaleZ ),
                new UiFloat3( "位置 (修改后)", RevisedValuesPosX, RevisedValuesPosY, RevisedValuesPosZ ),
                new UiFloat3( "旋转 (修改后)", RevisedValuesRotX, RevisedValuesRotY, RevisedValuesRotZ ),
                new UiFloat3( "颜色 (修改后)", RevisedValuesR, RevisedValuesG, RevisedValuesB ),
                IsDelayFastParticle,
                IsFitGround,
                IsTranformSkip,
                IsAllStopOnHide,
                CanBeClippedOut,
                ClipBoxenabled,
                new UiFloat3( "裁剪框位置", ClipBoxX, ClipBoxY, ClipBoxZ ),
                new UiFloat3( "裁剪框大小", ClipBoxSizeX, ClipBoxSizeY, ClipBoxSizeZ ),
                BiasZmaxScale,
                BiasZmaxDistance,
                IsCameraSpace,
                IsFullEnvLight,
                OSE,
                IsClipOwnSetting,
                NearClipBegin,
                NearClipEnd,
                FarClipBegin,
                FarClipEnd,
                SoftParticleFadeRange,
                SPHP,
                SoftKeyOffset,
                DrawLayerType,
                DrawOrderType,
                DirectionalLightSourceType,
                PointLightsType1,
                PointLightsType2,
                FadeEnabledX,
                FadeEnabledY,
                FadeEnabledZ,
                new UiFloat3( "内部淡化", FadeInnerX, FadeInnerY, FadeInnerZ ),
                new UiFloat3( "外部淡化", FadeOuterX, FadeOuterY, FadeOuterZ ),
                GlobalFogEnabled,
                GlobalFogInfluence,
                LTSEnabled,
                AGSEnabled,
                APRi,
                DPri,
                SABEnabled,
                SBVEnabled,
                SBVa,
                SSVEnabled,
                SSVa,
            ];
        }

        public override void ReadContents( BinaryReader reader, int size ) {
            Peek( reader, Parsed, size ); // read but then reset the position

            ReadNested( reader, ( _reader, _name, _size ) => {
                switch( _name ) {
                    case AvfxScheduler.NAME:
                        var Scheduler = new AvfxScheduler( File, NodeGroupSet );
                        Scheduler.Read( _reader, _size );
                        Schedulers.Add( Scheduler );
                        break;
                    case AvfxTimeline.NAME:
                        var Timeline = new AvfxTimeline( File, NodeGroupSet );
                        Timeline.Read( _reader, _size );
                        Timelines.Add( Timeline );
                        break;
                    case AvfxEmitter.NAME:
                        var Emitter = new AvfxEmitter( File, NodeGroupSet );
                        Emitter.Read( _reader, _size );
                        Emitters.Add( Emitter );
                        break;
                    case AvfxParticle.NAME:
                        var Particle = new AvfxParticle( File, NodeGroupSet );
                        Particle.Read( _reader, _size );
                        Particles.Add( Particle );
                        break;
                    case AvfxEffector.NAME:
                        var Effector = new AvfxEffector( File );
                        Effector.Read( _reader, _size );
                        Effectors.Add( Effector );
                        break;
                    case AvfxBinder.NAME:
                        var Binder = new AvfxBinder( File );
                        Binder.Read( _reader, _size );
                        Binders.Add( Binder );
                        break;
                    case AvfxTexture.NAME:
                        var Texture = new AvfxTexture( File );
                        Texture.Read( _reader, _size );
                        Textures.Add( Texture );
                        break;
                    case AvfxModel.NAME:
                        var Model = new AvfxModel( File );
                        Model.Read( _reader, _size );
                        Models.Add( Model );
                        break;
                }
            }, size );

            var versionBytes = BitConverter.GetBytes( Version.Value );
            for( var i = 0; i < versionBytes.Length; i++ ) UiVersion[i] = versionBytes[i];
            ScaleCombined = Math.Max( RevisedValuesScaleX.Value, Math.Max( RevisedValuesScaleY.Value, RevisedValuesScaleZ.Value ) );
        }

        public override void WriteContents( BinaryWriter writer ) {
            WriteNested( writer, Parsed );

            WriteLeaf( writer, "ScCn", 4, Schedulers.Count );
            WriteLeaf( writer, "TlCn", 4, Timelines.Count );
            WriteLeaf( writer, "EmCn", 4, Emitters.Count );
            WriteLeaf( writer, "PrCn", 4, Particles.Count );
            WriteLeaf( writer, "EfCn", 4, Effectors.Count );
            WriteLeaf( writer, "BdCn", 4, Binders.Count );
            WriteLeaf( writer, "TxCn", 4, Textures.Count );
            WriteLeaf( writer, "MdCn", 4, Models.Count );

            foreach( var item in Schedulers ) item.Write( writer );
            foreach( var item in Timelines ) item.Write( writer );
            foreach( var item in Emitters ) item.Write( writer );
            foreach( var item in Particles ) item.Write( writer );
            foreach( var item in Effectors ) item.Write( writer );
            foreach( var item in Binders ) item.Write( writer );
            foreach( var item in Textures ) item.Write( writer );
            foreach( var item in Models ) item.Write( writer );
        }

        protected override IEnumerable<AvfxBase> GetChildren() {
            foreach( var item in Parsed ) yield return item;
        }

        public override void Draw() {
            using var _ = ImRaii.PushId( "Avfx" );
            using var child = ImRaii.Child( "Child" );

            ImGui.TextDisabled( $"版本 {UiVersion[0]}.{UiVersion[1]}.{UiVersion[2]}.{UiVersion[3]}" );

            if( ImGui.InputFloat( "整体缩放 (修改后)", ref ScaleCombined ) ) {
                RevisedValuesScaleX.Value = ScaleCombined;
                RevisedValuesScaleY.Value = ScaleCombined;
                RevisedValuesScaleZ.Value = ScaleCombined;
            }
            ;

            ImGui.SameLine();
            UiUtils.HelpMarker( "修改后的位置、缩放和旋转仅会作用于未链接绑定器的效果。更多信息请查看\"绑定器\"一栏" );

            DrawItems( Display );
        }
    }
}
