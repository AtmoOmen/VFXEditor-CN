using Dalamud.Interface.Windowing;
using VfxEditor.Data.Copy;
using VfxEditor.FileManager.Interfaces;
using VfxEditor.Select;
using VfxEditor.Ui;

namespace VfxEditor.FileManager {
    public abstract class FileManagerBase : DalamudWindow, IFileManagerSelect {
        public readonly FileManagerGroupBase Group;
        public string FormatName => Group.FormatName;
        public string Title => Group.Title;
        public string Extension => Group.Extension;

        public readonly int WindowId;

        public readonly ManagerConfiguration Configuration;

        public readonly CopyManager Copy = new();

        public readonly WindowSystem WindowSystem = new();

        public abstract string NewWriteLocation { get; }

        public SelectDialog SourceSelect { get; protected set; }
        public SelectDialog ReplaceSelect { get; protected set; }

        protected FileManagerBase( FileManagerGroupBase group ) :
            base( $"{group.Title}##{group.WindowId}", true, new( 800, 1000 ), group.WindowSystem, isMainWindow: true ) {

            Group = group;
            WindowId = group.NewWindowId;
            Configuration = Plugin.Configuration.GetManagerConfig( FormatName );
        }

        public ManagerConfiguration GetConfig() => Configuration;

        public void ShowSource() => SourceSelect?.Show();

        public void ShowReplace() => ReplaceSelect?.Show();

        public abstract void SetSource( SelectResult result );

        public abstract void SetReplace( SelectResult result );

        public string GetId() => FormatName;

        public WindowSystem GetWindowSystem() => WindowSystem;

        public int GetWindowId() => WindowId;
    }
}
