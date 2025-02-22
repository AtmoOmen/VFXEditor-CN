﻿using FFXIVClientStructs.FFXIV.Client.Game.Character;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using VfxEditor.FileManager;
using VfxEditor.Formats.AtchFormat.Entry;
using VfxEditor.Utils;

namespace VfxEditor.Formats.AtchFormat {
    // https://github.com/Ottermandias/Penumbra.GameData/blob/main/Files/AtchFile.cs

    public class AtchFile : FileManagerFile {
        public static readonly Dictionary<string, string> WeaponNames = new() {
            { "2ax", "大斧" },
            { "2bk", "魔导书" },
            { "2bw", "弓" },
            { "2ff", "贤具" },
            { "2gb", "枪刃" },
            { "2gl", "天球仪" },
            { "2gn", "火枪" },
            { "2km", "镰刀" },
            { "2kt", "武士刀" },
            { "2rp", "刺剑" },
            { "2sp", "长枪" },
            { "2st", "巨杖" },
            { "2sw", "双手剑" },
            { "aai", "炼金术师" },
            { "aal", "炼金术师" },
            { "aar", "铸甲匠" },
            { "abl", "铸甲匠" },
            { "aco", "烹调师" },
            { "agl", "雕金匠" },
            { "ali", "炼金术师" },
            { "alm", "炼金术师" },
            { "alt", "制革匠" },
            { "ase", "裁衣匠" },
            // { "atr", "" },
            // { "avt", "" },
            { "awo", "刻木匠" },
            { "bag", "机工士弹匣" },
            { "chk", "战轮" },
            // { "clb", "" },
            { "clg", "手套" },
            // { "cls", "" }, // Linked to axes
            { "clw", "爪" },
            // { "col", "" },
            // { "cor", "" },
            // { "cos", "" },
            { "crd", "占星术士卡牌" },
            // { "crr", "" },
            // { "crt", "" },
            { "csl", "刻木匠" },
            { "csr", "刻木匠" },
            { "dgr", "忍者匕首" },
            { "drm", "鼓" },
            { "ebz", "钐镰客魂衣" },
            // { "egp", "" },
            // { "elg", "" },
            // { "fch", "" },
            // { "fdr", "" },
            { "fha", "捕鱼人" },
            // { "fl2", "Harp" },
            { "flt", "笛" },
            { "frg", "忍者烟雾" },
            { "fry", "制革匠/烹调师" },
            { "fsh", "捕鱼人" },
            { "fsw", "格斗武器" },
            // { "fud", "" },
            // { "gdb", "" },
            // { "gdh", "" },
            // { "gdl", "" }
            // { "gdr", "" },
            // { "gdt", "" },
            // { "gdw", "" },
            { "gsl", "机工士召唤物" },
            // { "gsr", "" }, // Diadem cannon?
            // { "gun", "" },
            // { "hel", "" },
            { "hmm", "锻铁匠/铸甲匠" },
            { "hrp", "吟游诗人乐器" },
            { "htc", "园艺工" },
            { "ksh", "武士刀鞘" },
            // { "let", "" },
            // { "lpr", "" }, // Linked to 1923
            { "mlt", "雕金匠" },
            { "mrb", "炼金术师" },
            { "mrh", "炼金术师" },
            { "msg", "机工士霰弹枪" },
            { "mwp", "机工士火枪" },
            { "ndl", "裁衣匠" },
            // { "nik", "" }, // Linked to Nier pod, maybe Nikana or something
            { "nph", "园艺工" },
            { "orb", "赤魔法师刺剑水晶" },
            // { "oum", "" },
            // { "pen", "" }, // Linked to daggers
            { "pic", "采矿工" },
            // { "pra", "" },
            { "prf", "制革匠" },
            { "qvr", "箭囊" },
            // { "rap", "" },
            { "rbt", "忍者兔子" },
            { "rod", "青魔杖" },
            // { "rop", "" },
            { "saw", "刻木匠" },
            // { "sht", "" },
            { "sic", "捕鱼人" },
            { "sld", "盾" },
            { "stf", "杖" },
            { "stv", "烹调师" },
            { "swd", "剑" },
            { "syl", "机工士狙击枪" },
            // { "syr", "" },
            // { "syu", "" },
            // { "tan", "" },
            { "tbl", "雕金匠" },
            // { "tcs", "" },
            { "tgn", "雕金匠" },
            { "tmb", "裁衣匠" },
            // { "trm", "" }, // Linked to flute
            // { "trr", "" },
            // { "trw", "" }, // Linked to greatswords
            // { "vln", "" },
            { "whl", "裁衣匠" },
            // { "wng", "" },
            // { "ypd", "" },
            { "ytk", "铸甲匠" },
        };

        public const int BitFieldSize = 32;

        public readonly ushort NumStates;
        public readonly List<AtchEntry> Entries = [];
        private readonly AtchEntrySplitView EntryView;

        public unsafe AtchFile( BinaryReader reader ) : base() {
            Verified = VerifiedStatus.UNSUPPORTED; // verifying these is fucked. The format is pretty simple though, so it's not a big deal

            var numEntries = reader.ReadUInt16();
            NumStates = reader.ReadUInt16();

            for( var i = 0; i < numEntries; i++ ) {
                Entries.Add( new( reader ) );
            }

            var bitfield = stackalloc ulong[BitFieldSize / 8];
            for( var i = 0; i < BitFieldSize / 8; ++i )
                bitfield[i] = reader.ReadUInt64();

            for( var i = 0; i < numEntries; ++i ) {
                var bitIdx = i & 0x3F;
                var ulongIdx = i >> 6;
                Entries[i].Accessory.Value = ( ( bitfield[ulongIdx] >> bitIdx ) & 1 ) == 1;
            }

            Entries.ForEach( x => x.ReadBody( reader, NumStates ) );
            EntryView = new( Entries );
        }

        public override void Write( BinaryWriter writer ) {
            writer.Write( ( ushort )Entries.Count );
            writer.Write( NumStates );

            Entries.ForEach( x => x.Write( writer ) );

            Span<byte> bitfield = stackalloc byte[BitFieldSize];
            foreach( var (entry, i) in Entries.WithIndex() ) {
                var bitIdx = i & 0x7;
                var byteIdx = i >> 3;
                if( Entries[i].Accessory.Value )
                    bitfield[byteIdx] |= ( byte )( 1 << bitIdx );
            }

            writer.Write( bitfield );

            var stringStartPos = 2 + 2 + ( 4 * Entries.Count ) + BitFieldSize + ( 32 * Entries.Count * NumStates );
            using var stringMs = new MemoryStream();
            using var stringWriter = new BinaryWriter( stringMs );
            var stringPos = new Dictionary<string, int>();

            Entries.ForEach( x => x.WriteBody( writer, stringStartPos, stringWriter, stringPos ) );

            writer.Write( stringMs.ToArray() );
        }

        public override void Draw() {
            DrawCurrentWeapons();

            ImGui.Separator();

            EntryView.Draw();
        }

        private unsafe void DrawCurrentWeapons() {
            if( Dalamud.ClientState == null || Plugin.PlayerObject == null ) return;

            var weapons = new List<string>();
            // https://github.com/aers/FFXIVClientStructs/blob/2c388216cb52d4b6c4dbdedb735e1b343d56a846/FFXIVClientStructs/FFXIV/Client/Game/Character/Character.cs#L78C20-L78C23
            var dataStart = ( nint )Unsafe.AsPointer( ref ( ( Character* )Plugin.PlayerObject.Address )->DrawData ) + 0x20;

            for( var i = 0; i < 3; i++ ) {
                var data = dataStart + ( DrawObjectData.Size * i );
                if( Marshal.ReadInt64( data + 8 ) == 0 || Marshal.ReadInt64( data + 16 ) == 0 || Marshal.ReadInt32( data + 32 ) == 0 ) continue;

                var nameArr = Marshal.PtrToStringAnsi( data + 32 ).ToCharArray();
                Array.Reverse( nameArr );
                weapons.Add( new string( nameArr ) );
            }

            if( weapons.Count == 0 ) return;

            ImGui.Separator();

            ImGui.TextDisabled( $"当前武器: {weapons.Aggregate( ( x, y ) => x + " | " + y )}" );
        }
    }
}
