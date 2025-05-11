using NAudio.Wave.SampleProviders;
using NAudio.Wave;
using PlaySound.Constants;
using PlaySound.Model;
using System;

namespace PlaySound.Helpers
{
    public class AudioPlaybackService : IDisposable
    {
        private WaveOutEvent outputDeviceVB = null!;
        private WaveOutEvent outputDeviceDefault = null!;
        private MixingSampleProvider mixerVB = null!;
        private MixingSampleProvider mixerDefault = null!;

        private const string VirtualCableDevice = "CABLE Input";
        
        private bool disposed;

        public AudioPlaybackService(int sampleRate = AudioConstants.DefaultSampleRate, int channelCount = AudioConstants.DefaultChannelCount)
        {
            InitializeDevices(sampleRate, channelCount);
        }

        public void PlaySoundVB(CachedSound sound)
        {
            outputDeviceVB.Volume = sound.Volume / 3;

            var cachedSound = new CachedSoundWaveProvider(sound);

            mixerVB.AddMixerInput(ConvertToRightChannelCount(cachedSound));
        }

        public void PlaySoundDefault(CachedSound sound)
        {
            outputDeviceDefault.Volume = sound.Volume;

            var cachedSound = new CachedSoundWaveProvider(sound);

            mixerDefault.AddMixerInput(ConvertToRightChannelCount(cachedSound));
        }
        public void StopAudio()
        {
            outputDeviceVB.Stop();
            outputDeviceDefault.Stop();

            InitializeDevices(48000, 2);
        }

        private void InitializeDevices(int sampleRate, int channelCount)
        {
            InitializeOutputDevices();
            InitializeMixers(sampleRate, channelCount);
            StartPlayback();
        }

        private void InitializeOutputDevices()
        {
            outputDeviceDefault = new WaveOutEvent();
            outputDeviceVB = new WaveOutEvent();
            FindAndSetVirtualCableDevice();
        }

        private void FindAndSetVirtualCableDevice()
        {
            for (int idx = 0; idx < WaveOut.DeviceCount; ++idx)
            {
                var device = WaveOut.GetCapabilities(idx);
                if (device.ProductName.Contains(AudioConstants.VirtualCableDevice))
                {
                    outputDeviceVB.DeviceNumber = idx;
                    break;
                }
            }
        }

        private void InitializeMixers(int sampleRate, int channelCount)
        {
            var waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount);
            mixerVB = new MixingSampleProvider(waveFormat);
            mixerDefault = new MixingSampleProvider(waveFormat);
            mixerVB.ReadFully = true;
            mixerDefault.ReadFully = true;
            outputDeviceVB.Init(mixerVB);
            outputDeviceDefault.Init(mixerDefault);
        }

        private void StartPlayback()
        {
            outputDeviceVB.Play();
            outputDeviceDefault.Play();
        }

        private IWaveProvider ConvertToRightChannelCount(IWaveProvider input)
        {
            return input.WaveFormat.Channels == mixerVB.WaveFormat.Channels
                ? input
                : input.WaveFormat.Channels == 1 && mixerVB.WaveFormat.Channels == 2
                    ? new MonoToStereoProvider16(input)
                    : throw new NotImplementedException("Not yet implemented this channel count conversion");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    outputDeviceVB.Dispose();
                    outputDeviceDefault.Dispose();
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public static readonly AudioPlaybackService Instance = new(48000, 2);
    }
}
