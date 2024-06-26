﻿using System;
using VfxEditor.Parsing;

namespace VfxEditor.ScdFormat {
    [Flags]
    public enum PointEnvironmentFlags {
        Use_Env_Filter_Depth = 0x40,
        Is_FireWorks = 0x80
    }

    [Flags]
    public enum PointFlags {
        Is_Reverb_Object = 0x40,
        Is_Whiz_Generate = 0x80
    }

    public class LayoutPointData : ScdLayoutData {
        public readonly ParsedFloat4 Position = new( "位置" );
        public readonly ParsedFloat MaxRange = new( "范围最大值" );
        public readonly ParsedFloat MinRange = new( "范围最小值" );
        public readonly ParsedFloat2 Height = new( "高度" );
        public readonly ParsedFloat RangeVolume = new( "范围音量" );
        public readonly ParsedFloat Volume = new( "音量" );
        public readonly ParsedFloat Pitch = new( "音高" );
        public readonly ParsedFloat ReverbFac = new( "混音强度系数" );
        public readonly ParsedFloat DopplerFac = new( "多普勒效应强度系数" );
        public readonly ParsedFloat CenterFac = new( "中心效应强度系数" );
        public readonly ParsedFloat InteriorFac = new( "内部效应强度系数" );
        public readonly ParsedFloat Direction = new( "方向" );
        public readonly ParsedFloat NearFadeStart = new( "近距离淡出开始" );
        public readonly ParsedFloat NearFadeEnd = new( "近距离淡出结束" );
        public readonly ParsedFloat FarDelayFac = new( "远距离延迟强度系数" );
        public readonly ParsedFlag<PointEnvironmentFlags> Environment = new( "Environment", size: 1 );
        public readonly ParsedFlag<PointFlags> Flag = new( "标识", size: 1 );
        public readonly ParsedReserve Reserved1 = new( 2 );
        public readonly ParsedFloat LowerLimit = new( "下限" );
        public readonly ParsedShort FadeInTime = new( "淡入时间" );
        public readonly ParsedShort FadeOutTime = new( "淡出时间" );
        public readonly ParsedFloat ConvergenceFac = new( "汇聚强度系数" );
        public readonly ParsedReserve Reserved2 = new( 4 );

        public LayoutPointData() {
            Parsed = [
                Position,
                MaxRange,
                MinRange,
                Height,
                RangeVolume,
                Volume,
                Pitch,
                ReverbFac,
                DopplerFac,
                CenterFac,
                InteriorFac,
                Direction,
                NearFadeStart,
                NearFadeEnd,
                FarDelayFac,
                Environment,
                Flag,
                Reserved1,
                LowerLimit,
                FadeInTime,
                FadeOutTime,
                ConvergenceFac,
                Reserved2
            ];
        }
    }
}
