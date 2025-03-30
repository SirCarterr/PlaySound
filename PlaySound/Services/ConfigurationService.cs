using Newtonsoft.Json;
using PlaySound.Interfaces;
using System;
using System.IO;

namespace PlaySound.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private const string SettingsFileName = "Settings.json";
        private readonly string _settingsFilePath;

        public string DirectoryPath { get; set; } = string.Empty;
        public string PlayAudioKey { get; set; } = string.Empty;
        public string StopAudioKey { get; set; } = string.Empty;
        public string NextAudioKey { get; set; } = string.Empty;
        public string PreviousAudioKey { get; set; } = string.Empty;

        // Constants
        public string VirtualCableDevice => "CABLE Input";
        public int DefaultSampleRate => 48000;
        public int DefaultChannelCount => 2;
        public int MaxFileSize => 3000000;
        public string ValidAudioExtension => ".mp3";
        public string DefaultHotKeyName => "None";
        public float DefaultVolume => 1.0f;

        public ConfigurationService()
        {
            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var projectDirectory = Directory.GetParent(appDirectory)!.Parent!.Parent!.Parent!.FullName;

            _settingsFilePath = Path.Combine(projectDirectory, "Files", SettingsFileName);
            LoadSettings();
        }

        public void SaveSettings()
        {
            var settings = new
            {
                DirectoryPath,
                PlayAudioKey,
                StopAudioKey,
                NextAudioKey,
                PreviousAudioKey
            };

            var json = JsonConvert.SerializeObject(settings);

            if (!Directory.Exists(Path.GetDirectoryName(_settingsFilePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_settingsFilePath)!);
            }

            File.WriteAllText(_settingsFilePath, json);
        }

        public void LoadSettings()
        {
            if (!File.Exists(_settingsFilePath))
            {
                SaveSettings();
                return;
            }

            var json = File.ReadAllText(_settingsFilePath);
            var settings = JsonConvert.DeserializeObject<ConfigurationService>(json);

            if (settings != null)
            {
                DirectoryPath = settings.DirectoryPath;
                PlayAudioKey = settings.PlayAudioKey;
                StopAudioKey = settings.StopAudioKey;
                NextAudioKey = settings.NextAudioKey;
                PreviousAudioKey = settings.PreviousAudioKey;
            }
        }
    }
} 