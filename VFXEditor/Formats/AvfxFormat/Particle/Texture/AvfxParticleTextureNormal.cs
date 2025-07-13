using System.Collections.Generic;
using System.IO;
using VfxEditor.AvfxFormat.Particle.Texture;
using VFXEditor.Formats.AvfxFormat.Curve;
using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxParticleTextureNormal : AvfxParticleAttribute {
        public readonly AvfxBool Enabled = new( "启用", "bEna" );
        public readonly AvfxInt UvSetIdx = new( "平面坐标集索引", "UvSN" );
        public readonly AvfxEnum<TextureFilterType> TextureFilter = new( "材质筛选器", "TFT" );
        public readonly AvfxEnum<TextureBorderType> TextureBorderU = new( "水平材质边界", "TBUT" );
        public readonly AvfxEnum<TextureBorderType> TextureBorderV = new( "垂直材质边界", "TBVT" );
        public readonly AvfxInt TextureIdx = new( "材质索引", "TxNo", value: -1 );
        public readonly AvfxCurve1Axis NPow = new( "强度", "NPow" );

        private readonly List<AvfxBase> Parsed;

        public AvfxParticleTextureNormal( AvfxParticle particle ) : base( "TN", particle, locked: true ) {
            InitNodeSelects();
            Display.Add( new TextureNodeSelectDraw( NodeSelects ) );

            Parsed = [
                Enabled,
                UvSetIdx,
                TextureFilter,
                TextureBorderU,
                TextureBorderV,
                TextureIdx,
                NPow
            ];

            Display.Add( Enabled );
            Display.Add( UvSetIdx );
            Display.Add( TextureFilter );
            Display.Add( TextureBorderU );
            Display.Add( TextureBorderV );

            DisplayTabs.Add( NPow );
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

        public override string GetDefaultText() => "法线材质";

        public override List<AvfxNodeSelect> GetNodeSelects() => [
            new AvfxNodeSelect<AvfxTexture>( Particle, "材质", Particle.NodeGroups.Textures, TextureIdx )
        ];
    }
}
