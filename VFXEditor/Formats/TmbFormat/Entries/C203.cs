﻿using System.Collections.Generic;
using VfxEditor.Parsing;
using VfxEditor.TmbFormat.Utils;

namespace VfxEditor.TmbFormat.Entries {
    public enum SummonWeaponObjectControl {
        Weapon = 0,
        OffHand = 1,
        Summon_or_Lemure = 2,
    }

    public class C203 : TmbEntry {
        public const string MAGIC = "C203";
        public const string DISPLAY_NAME = "贤具武器可见性";
        public override string DisplayName => DISPLAY_NAME;
        public override string Magic => MAGIC;

        public override int Size => 0x2C;
        public override int ExtraSize => 0;

        private readonly ParsedInt Duration = new( "持续时间" );
        private readonly ParsedInt Unk2 = new( "未知 2" );
        private readonly ParsedInt BindPointId = new( "绑定点 ID" );
        private readonly ParsedInt Rotation = new( "旋转" );
        private readonly ParsedEnum<SummonWeaponObjectControl> ObjectControl = new( "物体控制" );
        private readonly ParsedBool NoFollow = new( "禁用跟随" );
        private readonly ParsedBool ScaleEnabled = new( "启用缩放", size: 2 );
        private readonly ParsedShort Unk3 = new( "未知 3" );
        private readonly ParsedFloat Scale = new( "缩放" );

        public C203( TmbFile file ) : base( file ) { }

        public C203( TmbFile file, TmbReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => [
            Duration,
            Unk2,
            BindPointId,
            Rotation,
            ObjectControl,
            NoFollow,
            ScaleEnabled,
            Unk3,
            Scale
        ];
    }
}
