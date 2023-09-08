using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaySound.ViewModel.Helpers
{
    public class AudioPlayer
    {
        private Mp3FileReader? reader;
        private DirectSoundOut? soundOut;

        public event Action? PlaybackStopped;

        public AudioPlayer(string filepath)
        {
            reader = new Mp3FileReader(filepath);

            soundOut = new DirectSoundOut(200);
            soundOut.PlaybackStopped += Output_PlaybackStopped;

            var wc = new Wave32To16Stream(reader);
            soundOut.Init(wc);
        }

        public void Play()
        {
            var waveIn = new WaveInEvent { DeviceNumber = 0 };

            var waveOut = new WaveOut();
            waveOut.Init(new WaveInProvider(waveIn));
            waveOut.Play();

            byte[] buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = reader.Read(buffer, 0, buffer.Length)) > 0)
            {
                
            }

            soundOut?.Play();
        }

        public void Stop()
        {
            soundOut?.Stop();
        }

        private void Output_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            Dispose();
            PlaybackStopped?.Invoke();
        }

        private void Dispose()
        {
            if (soundOut != null)
            {
                if (soundOut.PlaybackState == PlaybackState.Playing)
                {
                    soundOut.Stop();
                }
                soundOut.Dispose();
                soundOut = null;
            }
            if (reader != null)
            {
                reader.Dispose();
                reader = null;
            }
        }
    }
}
