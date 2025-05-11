using NAudio.CoreAudioApi;
using PlaySound.Helpers;
using PlaySound.Interfaces;
using PlaySound.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace PlaySound.Services
{
    public class AudioManagerService : IAudioManagerService
    {
        private readonly GlobalHotKeyService _globalHotKeyService;
        private readonly AudioPlaybackService _audioPlaybackService;
        private readonly MMDeviceEnumerator _deviceEnumerator;
        private readonly MMDevice _defaultPlaybackDevice;

        private readonly List<CachedSound> _soundsVB = new();
        private readonly List<CachedSound> _soundsDefault = new();

        public AudioManagerService()
        {
            _globalHotKeyService = new GlobalHotKeyService();
            _deviceEnumerator = new MMDeviceEnumerator();
            _defaultPlaybackDevice = _deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            _audioPlaybackService = new AudioPlaybackService(_defaultPlaybackDevice.AudioClient.MixFormat.SampleRate);
        }

        public void InitializeHotKeys(IEnumerable<AudioDto> audioDtos)
        {
            InitializeSoundLists();
            RegisterDefaultHotKeys();
            RegisterAudioHotKeys(audioDtos);
        }

        private void InitializeSoundLists()
        {
            _soundsVB.Clear();
            _soundsDefault.Clear();
        }

        private void RegisterDefaultHotKeys()
        {
            _globalHotKeyService.UnregisterAllHotkeys();
            _globalHotKeyService.RegisterHotkey(ModifierKeys.Control, Key.End, StopAudio);
        }

        private void RegisterAudioHotKeys(IEnumerable<AudioDto> audioDtos)
        {
            foreach (var audio in audioDtos)
            {
                if (string.IsNullOrEmpty(audio.Path))
                    continue;

                _soundsVB.Add(new CachedSound(audio.Id, audio.Volume, audio.Path));
                _soundsDefault.Add(new CachedSound(audio.Id, audio.Volume, audio.Path));
                
                _globalHotKeyService.RegisterHotkey(audio.HotKey1, audio.HotKey2, () => PlayAudio(audio.Id));
            }
        }

        public void PlayAudio(AudioDto audio)
        {
            var (soundVB, soundDefault) = GetSoundsForAudio(audio.Id);
            _audioPlaybackService.PlaySoundVB(soundVB);
            _audioPlaybackService.PlaySoundDefault(soundDefault);
        }

        public void StopAudio(AudioDto audio)
        {
            _audioPlaybackService.StopAudio();
        }

        public void SetVolume(AudioDto audio, float volume)
        {
            var (soundVB, soundDefault) = GetSoundsForAudio(audio.Id);
            soundVB.Volume = volume;
            soundDefault.Volume = volume;
        }

        private (CachedSound soundVB, CachedSound soundDefault) GetSoundsForAudio(int audioId)
        {
            return (
                _soundsVB.First(s => s.Id == audioId),
                _soundsDefault.First(s => s.Id == audioId)
            );
        }

        private void PlayAudio(int Id)
        {
            var (soundVB, soundDefault) = GetSoundsForAudio(Id);
            _audioPlaybackService.PlaySoundVB(soundVB);
            _audioPlaybackService.PlaySoundDefault(soundDefault);
        }

        private void StopAudio()
        {
            _audioPlaybackService.StopAudio();
        }
    }
}
