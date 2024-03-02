using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using VfxEditor.Select.Data;

namespace VfxEditor.Select {
    public partial class SelectDataUtils {
        public static string BnpcPath => Path.Combine( Plugin.RootLocation, "Files", "bnpc.json" );
        public static string NpcFilesPath => Path.Combine( Plugin.RootLocation, "Files", "npc_files.json" );
        public static string CommonVfxPath => Path.Combine( Plugin.RootLocation, "Files", "common_vfx" );
        public static string CommonTmbPath => Path.Combine( Plugin.RootLocation, "Files", "common_tmb" );
        public static string CommonUldPath => Path.Combine( Plugin.RootLocation, "Files", "common_uld" );
        public static string CommonShpkPath => Path.Combine( Plugin.RootLocation, "Files", "common_shpk" );
        public static string CommonShcdPath => Path.Combine( Plugin.RootLocation, "Files", "common_shcd" );
        public static string CommonRacialPath => Path.Combine( Plugin.RootLocation, "Files", "common_racial" );

        [GeneratedRegex( "\\u0000([a-zA-Z0-9\\/_]*?)\\.avfx", RegexOptions.Compiled )]
        private static partial Regex AvfxRegexPattern();
        public static readonly Regex AvfxRegex = AvfxRegexPattern();

        [GeneratedRegex( "\\u0000([a-zA-Z0-9\\/_]*?)\\.scd", RegexOptions.Compiled )]
        private static partial Regex ScdRegexPattern();
        public static readonly Regex ScdRegex = ScdRegexPattern();

        // https://github.com/imchillin/CMTool/blob/master/ConceptMatrix/Views/SpecialControl.xaml.cs#L365

        public static readonly List<RacialData> CharacterRaces = new() {
            new RacialData( "中原之民男性", "c0101", 1 ),
            new RacialData( "中原之民女性","c0201", 101 ),
            new RacialData( "高地之民男性", "c0301", 201 ),
            new RacialData( "高地之民女性", "c0401", 301 ),
            new RacialData( "精灵族男性", "c0501", 401 ),
            new RacialData( "精灵族女性", "c0601", 501 ),
            new RacialData( "猫魅族男性", "c0701", 801 ),
            new RacialData( "猫魅族女性", "c0801", 901 ),
            new RacialData( "鲁加族男性", "c0901", 1001 ),
            new RacialData( "鲁加族女性", "c1001", 1101 ),
            new RacialData( "拉拉菲尔族男性", "c1101",601 ),
            new RacialData( "拉拉菲尔族女性", "c1201", 701 ),
            new RacialData( "敖龙族男性", "c1301", 1201 ),
            new RacialData( "敖龙族女性", "c1401", 1301 ),
            new RacialData( "硌狮族男性", "c1501", 1401 ),
            // 1601 coming soon (tm) - 25
            new RacialData( "维埃拉族男性", "c1701", 1601 ),
            new RacialData( "维埃拉族女性", "c1801", 1701 )
        };

        public static readonly Dictionary<string, string> JobAnimationIds = new() {
            { "战士", "bt_2ax_emp" },
            { "骑士", "bt_swd_sld" },
            { "绝枪战士", "bt_2gb_emp" },
            { "暗黑骑士", "bt_2sw_emp" },
            { "占星术士", "bt_2gl_emp" },
            { "贤者", "bt_2ff_emp" },
            { "学者", "bt_2bk_emp" },
            { "白魔法师", "bt_stf_sld" },
            { "机工士", "bt_2gn_emp" },
            { "舞者", "bt_chk_chk" },
            { "吟游诗人", "bt_2bw_emp" },
            { "武士", "bt_2kt_emp" },
            { "龙骑士", "bt_2sp_emp" },
            { "武僧", "bt_clw_clw" },
            { "忍者", "bt_dgr_dgr" },
            { "钐镰客", "bt_2km_emp" },
            { "赤魔法师", "bt_2rp_emp" },
            { "黑魔法师", "bt_jst_sld" },
            { "召唤师", "bt_2bk_emp" },
            { "青魔法师", "bt_rod_emp" },
        };

        public static readonly Dictionary<string, string> JobMovementOverride = new() {
            { "黑魔法师", "bt_stf_sld" },
            { "忍者", "bt_nin_nin" },
        };

        public static readonly Dictionary<string, string> JobDrawOverride = new() {
            { "黑魔法师", "bt_stf_sld" }
        };

        public static readonly Dictionary<string, string> JobAutoOverride = new() {
            { "黑魔法师", "bt_stf_sld" }
        };

        public static readonly int MaxChangePoses = 6;

        public static Dictionary<string, string> FileExistsFilter( Dictionary<string, string> dict ) => dict.Where( x => Dalamud.DataManager.FileExists( x.Value ) ).ToDictionary( x => x.Key, x => x.Value );

        public static string GetSkeletonPath( string skeletonId, string path ) => $"chara/human/{skeletonId}/animation/a0001/{path}";

        public static Dictionary<string, string> GetAllSkeletonPaths( string path ) {
            if( string.IsNullOrEmpty( path ) ) return new();
            return CharacterRaces.ToDictionary( x => x.Name, x => GetSkeletonPath( x.Id, path ) );
        }

        public static Dictionary<string, string> GetAllJobPaps( string jobId, string path ) => FileExistsFilter( GetAllSkeletonPaths( $"{jobId}/{path}.pap" ) );

        public static Dictionary<string, Dictionary<string, string>> GetAllJobPaps( string path ) {
            if( string.IsNullOrEmpty( path ) ) return new();
            return JobAnimationIds.ToDictionary( x => x.Key, x => GetAllJobPaps( x.Value, path ) );
        }

        public static Dictionary<string, Dictionary<string, string>> GetAllFacePaps( string path ) {
            if( string.IsNullOrEmpty( path ) ) return new();
            Dictionary<string, Dictionary<string, string>> ret = new();
            foreach( var item in CharacterRaces ) {
                ret[item.Name] = item.FaceOptions.ToDictionary( x => $"Face {x}", x => $"chara/human/{item.Id}/animation/f{x:D4}/nonresident/{path}.pap" );
            }
            return ret;
        }

        public static string ToTmbPath( string key ) => ( string.IsNullOrEmpty( key ) || key.Contains( "[SKL_ID]" ) ) ? string.Empty : $"chara/action/{key}.tmb";

        public static string ToVfxPath( string key ) => string.IsNullOrEmpty( key ) ? string.Empty : $"vfx/common/eff/{key}.avfx";
    }
}
