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
        }

        public async Task AddAudio(Audio audio)
        {
            _db.Audios.Add(audio);

            await _db.SaveChangesAsync();
        }

        public async Task<int> RemoveAudio(int id)
        {
            var audio = await _db.Audios
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            
            if (audio != null)
            {
                _db.Audios.Remove(audio);
                
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<int> UpdateAudio(Audio audio)
        {
            _db.ChangeTracker.Clear();
            
            var existingAudio = await _db.Audios
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == audio.Id);

            if (existingAudio == null)
            {
                return 0;
            }

            _db.Audios.Update(audio);
            return await _db.SaveChangesAsync();
        }

        public async Task<Audio> GetAudio(int id)
        {
            var audio = await _db.Audios
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            
            return audio ?? new();
        }

        public async Task<List<Audio>> GetAllAudios()
        {
            return await _db.Audios
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
