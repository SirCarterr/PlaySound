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
        public static Settings GetSettings()
        {
            Settings? settings;
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "settings.json")))
            {
                settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "settings.json")));
                if (settings != null)
                {
                    return settings;
                }
                settings = new Settings() 
                { 
                    DirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
                    PlayAudioKey = "+",
                    StopAudioKey = "-",
                    NextAudioKey = "right",
                    PreviousAudioKey = "left",
                };
                File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "settings.json"), JsonConvert.SerializeObject(settings));
                return settings;
            }
            settings = new() 
            {
                DirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
                PlayAudioKey = "+",
                StopAudioKey = "-",
                NextAudioKey = "right",
                PreviousAudioKey = "left",
            };
            using StreamWriter file = File.CreateText(Path.Combine(Environment.CurrentDirectory, "settings.json"));
            JsonSerializer serializer = new();
            serializer.Serialize(file, settings);
            return settings;
        }

        public static bool UpdateSettings(Settings settings)
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "settings.json")))
            {
                File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "settings.json"), JsonConvert.SerializeObject(settings));
                return true;
            }
            return false;
        }
    }
}
