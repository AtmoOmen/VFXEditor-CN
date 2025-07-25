using ImGuiNET;
using Lumina.Excel.Sheets;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VfxEditor.Select.Tabs.Npc {
    public struct NpcFilesStruct {
        public List<string> vfx;
        public List<string> tmb;
        public List<string> pap;

        public NpcFilesStruct() {
            vfx = [];
            tmb = [];
            pap = [];
        }
    }

    public abstract class NpcTab : SelectTab<NpcRow, List<string>> {
        private static Dictionary<string, NpcFilesStruct> NpcFiles = [];

        private readonly SelectResultType ResultType;

        public NpcTab( SelectDialog dialog, string name, SelectResultType resultType ) : base( dialog, name, "Npc" ) {
            ResultType = resultType;
        }

        public NpcTab( SelectDialog dialog, string name ) : this( dialog, name, SelectResultType.GameNpc ) { }

        // ===== LOADING =====

        public override void LoadData() {
            var nameToString = NameToString;

            // https://raw.githubusercontent.com/ffxiv-teamcraft/ffxiv-teamcraft/staging/libs/data/src/lib/json/gubal-bnpcs-index.json

            var baseToName = JsonConvert.DeserializeObject<Dictionary<string, uint>>( File.ReadAllText( SelectDataUtils.BnpcPath ) );
            var battleNpcSheet = Dalamud.DataManager.GetExcelSheet<BNpcBase>();
            foreach( var entry in baseToName ) {
                if( !nameToString.TryGetValue( entry.Value, out var name ) ) continue;
                if( !battleNpcSheet.TryGetRow( uint.Parse( entry.Key ), out var bnpcRow ) ) continue;
                if( !BnpcValid( bnpcRow ) ) continue;

                Items.Add( new NpcRow( bnpcRow.ModelChara.Value, name ) );
            }

            NpcFiles = JsonConvert.DeserializeObject<Dictionary<string, NpcFilesStruct>>( File.ReadAllText( SelectDataUtils.NpcFilesPath ) );
        }

        public override void LoadSelection( NpcRow item, out List<string> loaded ) {
            var files = NpcFiles.TryGetValue( item.ModelString, out var paths ) ? paths : new NpcFilesStruct();
            GetLoadedFiles( files, out loaded );
        }

        protected abstract void GetLoadedFiles( NpcFilesStruct files, out List<string> loaded );

        // ===== DRAWING ======

        protected override bool CheckMatch( NpcRow item, string searchInput ) =>
            SelectUiUtils.Matches( item.Name, searchInput ) || SelectUiUtils.Matches( item.ModelString, searchInput );

        protected override void DrawExtra() => SelectUiUtils.NpcThankYou();

        protected override void DrawSelected() {
            ImGui.TextDisabled( "分支: " + Selected.Variant );
            Dialog.DrawPaths( Loaded, Selected.Name, ResultType );
        }

        // ====== UTILS ===========

        public static Dictionary<uint, string> NameToString => Dalamud.DataManager.GetExcelSheet<BNpcName>()
                .Where( x => !string.IsNullOrEmpty( x.Singular.ExtractText() ) )
                .ToDictionary(
                    x => x.RowId,
                    x => x.Singular.ToString()
                );

        public static bool BnpcValid( BNpcBase? bnpcRow ) {
            if( bnpcRow?.ModelChara.ValueNullable == null || bnpcRow?.ModelChara.Value.Model == 0 ) return false;
            if( bnpcRow?.ModelChara.Value.Type != 2 && bnpcRow?.ModelChara.Value.Type != 3 ) return false;
            return true;
        }
    }
}