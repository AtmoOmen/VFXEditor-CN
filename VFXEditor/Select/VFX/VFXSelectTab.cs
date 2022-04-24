using VFXSelect.Select.Sheets;

namespace VFXSelect.VFX {
    public abstract class VFXSelectTab<T, S> : SelectTab<T, S> {
        protected readonly VFXSelectDialog Dialog;

        public VFXSelectTab( string parentId, string tabId, SheetLoader<T, S> loader, VFXSelectDialog dialog ) : base( parentId, tabId, loader ) {
            Dialog = dialog;
        }
    }
}