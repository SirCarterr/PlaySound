using NAudio.Wave.SampleProviders;
using NAudio.Wave;
using PlaySound.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaySound.ViewModel.Helpers
{
    public class AudioPlaybackService : IDisposable
    { 
        private WaveOutEvent outputDeviceVB;
        private WaveOutEvent outputDeviceDefault;
        private MixingSampleProvider mixerVB;
        private MixingSampleProvider mixerDefault;

        private readonly string virtualCableDevice = "CABLE Input";

        public AudioPlaybackService(int sampleRate = 48000, int channelCount = 2)
        {
            InitializeDevices(sampleRate, channelCount);
        }

        public void PlaySoundVB(CachedSound sound)
        {
            outputDeviceVB.Volume = sound.Volume / 3;
            AddMixerInputVB(new CachedSoundWaveProvider(sound));
        }

        public void PlaySoundDefault(CachedSound sound)
        {
            outputDeviceDefault.Volume = sound.Volume;
            AddMixerInputDefault(new CachedSoundWaveProvider(sound));
        }
        public void StopAudio()
        {
            outputDeviceVB.Stop();
            outputDeviceDefault.Stop();
            InitializeDevices(48000, 2);
        }

        private void InitializeDevices(int sampleRate, int channelCount)
        {
            outputDeviceDefault = new WaveOutEvent();
            outputDeviceVB = new WaveOutEvent();
            for (int idx = 0; idx < WaveOut.DeviceCount; ++idx)
            {
                var device = WaveOut.GetCapabilities(idx);
                if (device.ProductName.Contains(virtualCableDevice))
                {
                    outputDeviceVB.DeviceNumber = idx;
                    break;
                }
            }
            mixerVB = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount));
            mixerDefault = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount));
            mixerVB.ReadFully = true;
            mixerDefault.ReadFully = true;
            outputDeviceVB.Init(mixerVB);
            outputDeviceDefault.Init(mixerDefault);
            outputDeviceVB.Play();
            outputDeviceDefault.Play();
        }

        private IWaveProvider ConvertToRightChannelCount(IWaveProvider input)
        {
            if (input.WaveFormat.Channels == mixerVB.WaveFormat.Channels)
            {
                return input;
            }
            if (input.WaveFormat.Channels == 1 && mixerVB.WaveFormat.Channels == 2)
            {
                return new MonoToStereoProvider16(input);
            }
            throw new NotImplementedException("Not yet implemented this channel count conversion");
        }

        private void AddMixerInputVB(IWaveProvider input)
        {
            mixerVB.AddMixerInput(ConvertToRightChannelCount(input));
        }

        private void AddMixerInputDefault(IWaveProvider input)
        {
            mixerDefault.AddMixerInput(ConvertToRightChannelCount(input));
        }


        public void Dispose()
        {
            outputDeviceVB.Dispose();
            outputDeviceDefault.Dispose();
        }

        public static readonly AudioPlaybackService Instance = new AudioPlaybackService(48000, 2);
    }
}
