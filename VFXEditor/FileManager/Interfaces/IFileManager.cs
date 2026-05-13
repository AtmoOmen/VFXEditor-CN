using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using VfxEditor.Select;

namespace VfxEditor.FileManager.Interfaces {
    public interface IFileManager : IFileManagerSelect {
        public IEnumerable<IFileDocument> GetDocuments();

        public void Show();

        public void Draw();

        public void Reset( bool pluginClosing );
    }
}
