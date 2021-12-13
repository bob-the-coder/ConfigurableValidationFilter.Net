using System;
using System.IO;
using Newtonsoft.Json;

namespace BoxFilterExample
{
    public class LocalStorage<TObject>
    where TObject : class, new()
    {
        private readonly string _folderName;

        private string PathToRoot => 
            Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "/ConfigurableFilters");
        private string PathToFolder => Path.Join(PathToRoot, _folderName);
        private string PathToFile(string fileName) => Path.Join(PathToFolder, $"/{fileName}.json");

        public LocalStorage(string folderName)
        {
            _folderName = folderName;
            if (!Directory.Exists(PathToRoot)) Directory.CreateDirectory(PathToRoot);
        }

        public void Save(string name, TObject obj)
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);

            if (!Directory.Exists(PathToFolder)) Directory.CreateDirectory(PathToFolder);
            var filePath = PathToFile(name);

            if (File.Exists(filePath)) File.Delete(filePath);
            
            using var streamWriter = File.CreateText(filePath);
            streamWriter.Write(json);
            streamWriter.Close();
        }

        public TObject Load(string id)
        {
            if (!Directory.Exists(PathToFolder)) return null;

                var filePath = PathToFile(id);
            if (!File.Exists(filePath)) return null;

            using var streamReader = new StreamReader(filePath);
            var json = streamReader.ReadToEnd();

            return JsonConvert.DeserializeObject<TObject>(json);
        }

        public void Delete(string id)
        {
            if (!Directory.Exists(PathToFolder)) return;
            var filePath = PathToFile(id);

            if (File.Exists(filePath)) File.Delete(filePath);
        }
    }
}
