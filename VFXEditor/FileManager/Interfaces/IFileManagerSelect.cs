using Dalamud.Interface.Windowing;

namespace VfxEditor.FileManager.Interfaces {
    public interface IFileManagerSelect {
        public string GetId();

        public int GetWindowId();

        public ManagerConfiguration GetConfig();

        public WindowSystem GetWindowSystem();
    }
}
