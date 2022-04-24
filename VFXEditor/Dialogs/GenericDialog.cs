using ImGuiNET;
using System.Numerics;

namespace VFXEditor.Dialogs {

    public abstract class GenericDialog {
        protected bool Visible = false;
        protected string DialogName;
        protected Vector2 Size = new( 600, 400 );
        protected bool MenuBar = false;

        public bool IsVisible => Visible;

        public GenericDialog( string name, bool menuBar = false ) {
            DialogName = name;
            MenuBar = menuBar;
        }

        public void Show() {
            Visible = true;
        }

        public void Hide() {
            Visible = false;
        }

        public void Toggle() {
            Visible = !Visible;
        }

        public void SetVisible( bool visible ) {
            Visible = visible;
        }

        public void Draw() {
            if( !Visible ) return;
            ImGui.SetNextWindowSize( Size, ImGuiCond.FirstUseEver );

            if( ImGui.Begin( DialogName, ref Visible, ( MenuBar ? ImGuiWindowFlags.MenuBar : ImGuiWindowFlags.None ) | ImGuiWindowFlags.NoDocking ) ) {
                DrawBody();
            }
            ImGui.End();
        }

        public abstract void DrawBody();
    }
}
