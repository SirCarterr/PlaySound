using NAudio.Wave;
using PlaySound.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaySound.ViewModel.Helpers
{
    public class CachedSoundWaveProvider : IWaveProvider
    {
        private readonly CachedSound cachedSound;
        private long position;

        public CachedSoundWaveProvider(CachedSound cachedSound)
        {
            this.cachedSound = cachedSound;
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            var availableSamples = cachedSound.AudioData.Length - position;
            var samplesToCopy = Math.Min(availableSamples, count);
            Array.Copy(cachedSound.AudioData, position, buffer, offset, samplesToCopy);
            position += samplesToCopy;
            return (int)samplesToCopy;
        }

        public WaveFormat WaveFormat { get { return cachedSound.WaveFormat; } }
    }
}
