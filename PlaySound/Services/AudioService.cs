using PlaySound.Converter;
using PlaySound.Model;
using PlaySound.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlaySound.Services
{
    public class AudioService
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

        public async Task<int> RemoveAudio(int id)
        {
            return await _audioRepository.RemoveAudio(id);
        }

        public async Task<int> UpdateAudio(AudioDto audioDto)
        {
            var audio = AudioConverter.ConvertFromDTO(audioDto);
            
            return await _audioRepository.UpdateAudio(audio);
        }

        public async Task<AudioDto> GetAudio(int id)
        {
            var audio = await _audioRepository.GetAudio(id);
            
            return AudioConverter.ConvertToDTO(audio);
        }

        public async Task<List<AudioDto>> GetAllAudios()
        {
            var audios = await _audioRepository.GetAllAudios();
            
            var audioDtos = new List<AudioDto>();
            
            foreach (var audio in audios)
            {
                audioDtos.Add(AudioConverter.ConvertToDTO(audio));
            }
            
            return audioDtos;
        }
    }
}
