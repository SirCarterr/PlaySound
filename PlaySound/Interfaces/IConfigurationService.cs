namespace PlaySound.Interfaces
{
    public interface IConfigurationService
    {
        string DirectoryPath { get; set; }
        string PlayAudioKey { get; set; }
        string StopAudioKey { get; set; }
        string NextAudioKey { get; set; }
        string PreviousAudioKey { get; set; }
        string VirtualCableDevice { get; }
        int DefaultSampleRate { get; }
        int DefaultChannelCount { get; }
        int MaxFileSize { get; }
        string ValidAudioExtension { get; }
        string DefaultHotKeyName { get; }
        float DefaultVolume { get; }
        void SaveSettings();
        void LoadSettings();
    }
} 