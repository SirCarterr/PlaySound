using Newtonsoft.Json;
using PlaySound.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlaySound.ViewModel.Helpers
{
    public static class SettingsHelper
    {
        public static Settings? GetSettings()
        {
            if(File.Exists(Path.Combine(Environment.CurrentDirectory, "Files", "Settings.json")))
            {
                return JsonConvert.DeserializeObject<Settings>(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Files", "Settings.json")));
            }
            Settings settings = new() { DirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) };
            using StreamWriter file = File.CreateText(Path.Combine(Environment.CurrentDirectory, "Files", "Settings.json"));
            JsonSerializer serializer = new();
            serializer.Serialize(file, settings);
            return settings;
        }

        public static bool UpdateSettings(Settings settings)
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "Files", "Settings.json")))
            {
                File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "Files", "Settings.json"), JsonConvert.SerializeObject(settings));
                return true;
            }
            return false;
        }
    }
}
