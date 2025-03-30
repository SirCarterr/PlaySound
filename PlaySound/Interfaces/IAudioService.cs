using System.Collections.Generic;
using System.Threading.Tasks;
using PlaySound.Model;

namespace PlaySound.Interfaces
{
    public interface IAudioService
    {
        Task<IEnumerable<AudioDto>> GetAllAudios();
        Task AddAudio(AudioDto audio);
        Task UpdateAudio(AudioDto audio);
        Task RemoveAudio(int id);
    }
} 