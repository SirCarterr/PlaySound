using Microsoft.EntityFrameworkCore;
using PlaySound.Data;
using PlaySound.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlaySound.Repositories
{
    public class AudioRepository
    {
        private readonly ApplicationContext _db;

        public AudioRepository()
        {
            _db = new ApplicationContext();
            
            _db.Database.EnsureCreated();
            _db.Audios.Load();
        }

        public async Task AddAudio(Audio audio)
        {
            _db.Audios.Add(audio);

            await _db.SaveChangesAsync();
        }

        public async Task<int> RemoveAudio(int id)
        {
            var audio = await _db.Audios.FirstOrDefaultAsync(x => x.Id == id);
            
            if (audio != null)
            {
                _db.Audios.Remove(audio);
                
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<int> UpdateAudio(Audio audio)
        {
            var entry = _db.Entry(audio);

            entry.State = EntityState.Modified;

            return await _db.SaveChangesAsync();
        }

        public async Task<Audio> GetAudio(int id)
        {
            var audio = await _db.Audios.FirstOrDefaultAsync(x => x.Id == id);
            
            return audio ?? new();
        }

        public async Task<List<Audio>> GetAllAudios()
        {
            var audios = await _db.Audios.ToListAsync();

            return audios;
        }
    }
}
