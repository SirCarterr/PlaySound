using System.Collections.Generic;
using System.Threading.Tasks;
using PlaySound.Model;

namespace PlaySound.Interfaces
{
    public interface IAudioManagerService
    {
        void InitializeHotKeys(IEnumerable<AudioDto> audios);
        void PlayAudio(AudioDto audio);
        void StopAudio(AudioDto audio);
        void SetVolume(AudioDto audio, float volume);
    }
} 