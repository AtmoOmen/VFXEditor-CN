using System.Collections.Generic;
using System.IO;
using VfxEditor.AvfxFormat.Particle.Texture;
using VFXEditor.Formats.AvfxFormat.Curve;
using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxParticleTexturePalette : AvfxParticleAttribute {
        public readonly AvfxBool Enabled = new( "启用", "bEna" );
        public readonly AvfxEnum<TextureFilterType> TextureFilter = new( "材质筛选器", "TFT" );
        public readonly AvfxEnum<TextureBorderType> TextureBorder = new( "材质边框", "TBT" );
        public readonly AvfxInt TextureIdx = new( "材质索引", "TxNo", value: -1 );
        public readonly AvfxCurve1Axis Offset = new( "偏移", "POff" );
        public readonly AvfxCurve1Axis OffsetRandom = new( "随机偏移", "POfR" );

        private readonly List<AvfxBase> Parsed;

        public AvfxParticleTexturePalette( AvfxParticle particle ) : base( "TP", particle, locked: true )
        {
            InitNodeSelects();
            Display.Add( new TextureNodeSelectDraw( NodeSelects ) );

            Parsed = [
                Enabled,
                TextureFilter,
                TextureBorder,
                TextureIdx,
                Offset,
                OffsetRandom
            ];

            Display.Add( Enabled );
            Display.Add( TextureFilter );
            Display.Add( TextureBorder );

            DisplayTabs.Add( Offset );
            DisplayTabs.Add( OffsetRandom );
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

        public override string GetDefaultText() => "材质调色";

        public override List<AvfxNodeSelect> GetNodeSelects() => [
            new AvfxNodeSelect<AvfxTexture>( Particle, "材质", Particle.NodeGroups.Textures, TextureIdx )
        ];
    }
}
