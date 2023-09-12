using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace PlaySound.ViewModel.Helpers
{
    public class AudioPlayer : IDisposable
    {
        private readonly WaveOutEvent soundOutVB;
        private readonly WaveOutEvent soundOutDefault;

        private readonly MixingSampleProvider sampleProviderVB;
        //private readonly MixingSampleProvider sampleProviderDefault;

        public AudioPlayer()
        {
            string virtualCableDevice = "CABLE Input";

            sampleProviderVB = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(48000, 2));
            //sampleProviderDefault = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(48000, 2));

            sampleProviderVB.ReadFully = true; 
            //sampleProviderDefault.ReadFully = true;

            soundOutVB = new WaveOutEvent();
            soundOutDefault = new WaveOutEvent();

            for (int idx = 0; idx < WaveOut.DeviceCount; ++idx)
            {
                var device = WaveOut.GetCapabilities(idx);
                if (device.ProductName.Contains(virtualCableDevice))
                {
                    soundOutVB.DeviceNumber = idx;
                    break;
                }
            }

            if (soundOutVB.DeviceNumber == -1)
            {
                MessageBox.Show("The \"VB-Audio Virtal Cable\" is not downoloaded or activated and thus audio will not be played in microphone!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            soundOutVB.Init(sampleProviderVB);
            soundOutDefault.Init(sampleProviderVB);
            soundOutVB.Play();
            soundOutDefault.Play();
        }

        public void Play(string audioFilePath)
        {
            if(!File.Exists(audioFilePath))
            {
                MessageBox.Show("File does not exists:\n" + audioFilePath, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            using var readerVb = new Mp3FileReader(audioFilePath);
            using var readerDefault = new Mp3FileReader(audioFilePath);

            int outRate = 48000;
            var outFormatVB = new WaveFormat(outRate, readerVb.WaveFormat.Channels);
            var outFormatDefault = new WaveFormat(outRate, readerDefault.WaveFormat.Channels);
            using var resamplerVB = new MediaFoundationResampler(readerVb, outFormatVB);
            using var resamplerDefault = new MediaFoundationResampler(readerDefault, outFormatDefault);

            var sampleVB = new Wave16ToFloatProvider(resamplerVB);
            var sampleVDefault = new Wave16ToFloatProvider(resamplerDefault);  

            sampleProviderVB.AddMixerInput(sampleVB.ToSampleProvider());
            //sampleProviderDefault.AddMixerInput(sampleVDefault.ToSampleProvider());

            //soundOutVB.Init(readerVb);
            //soundOutDefault.Init(readerDefault);
            //soundOutVB.Play();

            //soundOutDefault.Play();

            //Stop();
        }

        public void Dispose()
        {
            soundOutVB.Dispose();
            soundOutDefault.Dispose();
        }

        //public void Stop()
        //{
        //    soundOutVB?.Stop();
        //    soundOutDefault?.Stop();
        //}
    }
}
