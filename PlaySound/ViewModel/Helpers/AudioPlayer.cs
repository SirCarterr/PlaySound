using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlaySound.ViewModel.Helpers
{
    public class AudioPlayer
    {
        private readonly WaveOut soundOutVB;
        private readonly WaveOut soundOutDefault;

        public AudioPlayer()
        {
            string virtualCableDevice = "CABLE Input (VB-Audio Virtual C";

            soundOutVB = new WaveOut();
            soundOutDefault = new WaveOut();

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
                MessageBox.Show("The \"VB-Audio Virtal Cable\" did not downoloaded or activated and thus audio will not be played in microphone!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void Play(string audioFilePath)
        {
            if(!File.Exists(audioFilePath))
            {
                MessageBox.Show("File does not exists:\n" + audioFilePath, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            using var readerVb = new Mp3FileReader(audioFilePath);
            using var readerDefault = new Mp3FileReader(audioFilePath);

            soundOutVB.Init(readerVb);
            soundOutDefault.Init(readerDefault);
            soundOutVB.Play();
            soundOutDefault.Play();

            //Stop();
        }

        public void Stop()
        {
            soundOutVB?.Stop();
            soundOutDefault?.Stop();
            soundOutVB?.Dispose();
            soundOutDefault?.Dispose();
        }
    }
}
