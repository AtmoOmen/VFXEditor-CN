using System.Collections.Generic;
using System.IO;
using VfxEditor.AvfxFormat.Particle.Texture;
using VFXEditor.Formats.AvfxFormat.Curve;
using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxParticleTextureColor1 : AvfxParticleAttribute {
        public readonly AvfxBool Enabled = new( "启用", "bEna" );
        public readonly AvfxBool ColorToAlpha = new( "颜色值转透明度", "bC2A" );
        public readonly AvfxBool UseScreenCopy = new( "使用屏幕复制", "bUSC" );
        public readonly AvfxBool PreviousFrameCopy = new( "复制前一帧", "bPFC" );
        public readonly AvfxInt UvSetIdx = new( "平面坐标集索引", "UvSN" );
        public readonly AvfxEnum<TextureFilterType> TextureFilter = new( "材质筛选器", "TFT" );
        public readonly AvfxEnum<TextureBorderType> TextureBorderU = new( "水平材质边界", "TBUT" );
        public readonly AvfxEnum<TextureBorderType> TextureBorderV = new( "垂直材质边界", "TBVT" );
        public readonly AvfxEnum<TextureCalculateColor> TextureCalculateColor = new( "颜色计算方式", "TCCT" );
        public readonly AvfxEnum<TextureCalculateAlpha> TextureCalculateAlpha = new( "透明度计算方式", "TCAT" );
        public readonly AvfxInt TextureIdx = new( "材质索引", "TxNo", value: -1 );
        public readonly AvfxBool UOS = new( "UOS", "bUOS" );
        public readonly AvfxIntList MaskTextureIdx = new( "遮罩索引", "TLst", value: -1 );
        public readonly AvfxCurve1Axis TexN = new( "TexN", "TxN" );
        public readonly AvfxCurve1Axis TexNRandom = new( "随机 TexN", "TxNR" );

        private readonly List<AvfxBase> Parsed;

        public AvfxParticleTextureColor1( AvfxParticle particle ) : base( "TC1", particle ) {
            InitNodeSelects();
            Display.Add( new TextureNodeSelectDraw( NodeSelects ) );

            Parsed = [
                Enabled,
                ColorToAlpha,
                UseScreenCopy,
                PreviousFrameCopy,
                UvSetIdx,
                TextureFilter,
                TextureBorderU,
                TextureBorderV,
                TextureCalculateColor,
                TextureCalculateAlpha,
                TextureIdx,
                UOS,
                MaskTextureIdx,
                TexN,
                TexNRandom
            ];

            Display.Add( Enabled );
            Display.Add( ColorToAlpha );
            Display.Add( UseScreenCopy );
            Display.Add( PreviousFrameCopy );
            Display.Add( UvSetIdx );
            Display.Add( TextureFilter );
            Display.Add( TextureBorderU );
            Display.Add( TextureBorderV );
            Display.Add( TextureCalculateColor );
            Display.Add( TextureCalculateAlpha );
            Display.Add( UOS );
            DisplayTabs.Add( TexN );
            DisplayTabs.Add( TexNRandom );
        }

        public override void ReadContents( BinaryReader reader, int size ) {
            ReadNested( reader, Parsed, size );
            EnableAllSelectors();
        }

        public override void WriteContents( BinaryWriter writer ) => WriteNested( writer, Parsed );

        protected override IEnumerable<AvfxBase> GetChildren() {
            foreach( var item in Parsed ) yield return item;
        }

        public override void DrawBody() {
            DrawNamedItems( DisplayTabs );
        }

        public override string GetDefaultText() => "材质颜色 1";

        public override List<AvfxNodeSelect> GetNodeSelects() => [
            new AvfxNodeSelectList<AvfxTexture>( Particle, "遮罩材质", Particle.NodeGroups.Textures, MaskTextureIdx, 256 )
        ];
    }
}
