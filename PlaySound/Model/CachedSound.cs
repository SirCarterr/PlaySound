using NAudio.CoreAudioApi;
using NAudio.Wave;
using System.Collections.Generic;
using System.Linq;

namespace PlaySound.Model
{
    public class CachedSound
    {
        public int Id { get; set; }
        public float Volume { get; set; }
        public byte[] AudioData { get; private set; }
        public WaveFormat WaveFormat { get; private set; }

        public CachedSound(int id, float volume, string audioFileName)
        {
            Id = id;
            Volume = volume;

            using var audioFileReader = new Mp3FileReader(audioFileName);
            
            var outFormat = GetOutputFormat(audioFileReader);
            
            WaveFormat = outFormat;
            AudioData = ReadAudioData(audioFileReader, outFormat);
        }

        private static WaveFormat GetOutputFormat(Mp3FileReader audioFileReader)
        {
            var enumerator = new MMDeviceEnumerator();
            
            var defaultPlaybackDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            return new WaveFormat(defaultPlaybackDevice.AudioClient.MixFormat.SampleRate, audioFileReader.WaveFormat.Channels);
        }

        private static byte[] ReadAudioData(Mp3FileReader audioFileReader, WaveFormat outFormat)
        {
            using var resampler = new MediaFoundationResampler(audioFileReader, outFormat);
            
            var wholeFile = new List<byte>((int)(audioFileReader.Length / 4));
            var readBuffer = new byte[resampler.WaveFormat.SampleRate * resampler.WaveFormat.Channels];

            int samplesRead;
            while ((samplesRead = resampler.Read(readBuffer, 0, readBuffer.Length)) > 0)
            {
                wholeFile.AddRange(readBuffer.Take(samplesRead));
            }

            return wholeFile.ToArray();
        }
    }
}
