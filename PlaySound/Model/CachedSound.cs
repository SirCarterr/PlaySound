using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            using (var audioFileReader = new Mp3FileReader(audioFileName))
            {
                // TODO: could add resampling in here if required
                MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
                MMDevice defaultPlaybackDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
                int outRate = defaultPlaybackDevice.AudioClient.MixFormat.SampleRate;
                //int outRate = 48000;
                var outFormat = new WaveFormat(outRate, audioFileReader.WaveFormat.Channels);
                using var resampler = new MediaFoundationResampler(audioFileReader, outFormat);
                WaveFormat = resampler.WaveFormat;
                var wholeFile = new List<byte>((int)(audioFileReader.Length / 4));
                var readBuffer = new byte[resampler.WaveFormat.SampleRate * resampler.WaveFormat.Channels];
                int samplesRead;
                while ((samplesRead = resampler.Read(readBuffer, 0, readBuffer.Length)) > 0)
                {
                    wholeFile.AddRange(readBuffer.Take(samplesRead));
                }
                AudioData = wholeFile.ToArray();
            }
        }
    }
}
