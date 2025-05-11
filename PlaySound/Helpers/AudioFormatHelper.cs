using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace PlaySound.Helpers
{
    public static class AudioFormatHelper
    {
        public static WaveFormat GetOutputFormat(WaveFormat inputFormat)
        {
            var enumerator = new MMDeviceEnumerator();
            var defaultPlaybackDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            return new WaveFormat(defaultPlaybackDevice.AudioClient.MixFormat.SampleRate, inputFormat.Channels);
        }
    }
} 