using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PlaySound.Model;
using PlaySound.ViewModel.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaySound.ViewModel.Helpers
{
    public class AudioManager
    {
        private readonly ApplicationContext _db;

        public AudioManager()
        {
            _db = new ApplicationContext();
            _db.Database.EnsureCreated();
            _db.Audios.Load();
        }

        public void AddAudio(Audio audio)
        {
            _db.Audios.Add(audio);
            _db.SaveChanges();
        }

        public int RemoveAudio(int id)
        {
            Audio? audio = _db.Audios.FirstOrDefault(x => x.Id == id);
            if (audio != null)
            {
                _db.Audios.Remove(audio);
                return _db.SaveChanges();
            }
            return 0;
        }

        public int UpdateAudio(Audio audio)
        {
            Audio? audio_db = _db.Audios.FirstOrDefault(x => x.Id == audio.Id);
            if (audio_db != null)
            {
                audio_db.Path = audio.Path;
                audio_db.Name = audio.Name;
                audio_db.HotKey1 = audio.HotKey1;
                audio_db.HotKey2 = audio.HotKey2;
                _db.Audios.Update(audio_db);
                return _db.SaveChanges();
            }
            return 0;
        }

        public Audio GetAudio(int id)
        {
            Audio? audio = _db.Audios.FirstOrDefault(x => x.Id == id);
            if (audio != null)
            {
                return audio;
            }
            return new();
        }

        public List<Audio> GetAllAudios()
        {
            return _db.Audios.ToList();
        }
    }
}
