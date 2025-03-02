using NAudio.CoreAudioApi;
using PlaySound.Helpers;
using PlaySound.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace PlaySound.Services
{
    public class AudioManagerService
    {
        private readonly GlobalHotKeyService _globalHotKeyService;
        private readonly AudioPlaybackService _audioPlaybackService;

        private readonly List<CachedSound> _soundsVB = new();
        private readonly List<CachedSound> _soundsDefault = new();

        public AudioManagerService()
        {
            _globalHotKeyService = new GlobalHotKeyService();

            var enumerator = new MMDeviceEnumerator();
            var defaultPlaybackDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

            var outRate = defaultPlaybackDevice.AudioClient.MixFormat.SampleRate;

            _audioPlaybackService = new AudioPlaybackService(outRate);
        }

        public void InitializeHotKeys(List<AudioDto> audioDtos)
        {
            _soundsVB.Clear();
            _soundsDefault.Clear();
            
            _globalHotKeyService.UnregisterAllHotkeys();
            _globalHotKeyService.RegisterHotkey(ModifierKeys.Control, Key.End, StopAudio);

            foreach (var audio in audioDtos)
            {
                if (string.IsNullOrEmpty(audio.Path))
                    continue;

                _soundsVB.Add(new CachedSound(audio.Id, audio.Volume, audio.Path));
                _soundsDefault.Add(new CachedSound(audio.Id, audio.Volume, audio.Path));
                
                _globalHotKeyService.RegisterHotkey(audio.HotKey1, audio.HotKey2, () => PlayAudio(audio.Id));
            }
        }

        private void PlayAudio(int Id)
        {
            var soundVB = _soundsVB.First(s => s.Id == Id);
            var soundDefault = _soundsDefault.First(s => s.Id == Id);

            _audioPlaybackService.PlaySoundVB(soundVB);
            _audioPlaybackService.PlaySoundDefault(soundDefault);
        }

        private void StopAudio()
        {
            _audioPlaybackService.StopAudio();
        }
    }
}
