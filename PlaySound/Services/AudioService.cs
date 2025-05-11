using PlaySound.Converter;
using PlaySound.Interfaces;
using PlaySound.Model;
using PlaySound.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace PlaySound.Services
{
    public class AudioService : IAudioService
    {
        private readonly AudioRepository _audioRepository;

        public AudioService() 
        {
            _audioRepository = new AudioRepository();
        }

        public async Task AddAudio(AudioDto audioDto)
        {
            var audio = AudioConverter.ConvertFromDTO(audioDto);
            
            await _audioRepository.AddAudio(audio);
        }

        public async Task RemoveAudio(int id)
        {
            await _audioRepository.RemoveAudio(id);
        }

        public async Task UpdateAudio(AudioDto audioDto)
        {
            var audio = AudioConverter.ConvertFromDTO(audioDto);
            
            await _audioRepository.UpdateAudio(audio);
        }

        public async Task<AudioDto> GetAudio(int id)
        {
            var audio = await _audioRepository.GetAudio(id);
            
            return AudioConverter.ConvertToDTO(audio);
        }

        public async Task<IEnumerable<AudioDto>> GetAllAudios()
        {
            var audios = await _audioRepository.GetAllAudios();
            return audios.Select(AudioConverter.ConvertToDTO);
        }
    }
}
