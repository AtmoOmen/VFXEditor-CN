namespace VFXSelect.Select.Rows {
    public class XivItemSelected {
        public XivItem Item;

        public int Count;
        public bool VfxExists;
        public int VfxId;
        public string ImcPath;

        public XivItemSelected( Lumina.Data.Files.ImcFile file, XivItem item ) {
            Item = item;
            Count = file.Count;
            var imcData = file.GetVariant( item.Variant - 1 );
            ImcPath = file.FilePath;
            VfxId = imcData.VfxId;
            VfxExists = !( VfxId == 0 );
        }

        public string GetVFXPath() {
            if( !VfxExists ) return "";
            return Item.GetVFXPath( VfxId );
        }
    }
}
