﻿using System;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Parsing;

namespace VfxEditor.PhybFormat.Simulator {
    [Flags]
    public enum SimulatorFlags {
        Simulating = 0x01,
        Collisions_Handled = 0x02,
        Continuous_Collisions = 0x04,
        Using_Ground_Plane = 0x08,
        Fixed_Length = 0x10,
    }

    public class PhybSimulatorParams : PhybData {
        public readonly ParsedFloat3 Gravity = new( "重力" );
        public readonly ParsedFloat3 Wind = new( "风" );
        public readonly ParsedShort ConstraintLoop = new( "约束循环" );
        public readonly ParsedShort CollisionLoop = new( "碰撞循环" );
        public readonly ParsedFlag<SimulatorFlags> Flags = new( "标识", size: 1 );
        public readonly ParsedByte Group = new( "组" );
        public readonly ParsedReserve Padding = new( 2 );

        public PhybSimulatorParams( PhybFile file ) : base( file ) { }

        public PhybSimulatorParams( PhybFile file, BinaryReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => [
            Gravity,
            Wind,
            ConstraintLoop,
            CollisionLoop,
            Flags,
            Group,
            Padding,
        ];
    }
}
