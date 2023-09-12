using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PlaySound.Common;
using PlaySound.Model;
using PlaySound.ViewModel.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

        public void AddAudio(AudioDTO audio)
        {
            _db.Audios.Add(ConvertFromDTO(audio));
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

        public int UpdateAudio(AudioDTO audio)
        {
            Audio? audio_db = _db.Audios.FirstOrDefault(x => x.Id == audio.Id);
            if (audio_db != null)
            {
                audio_db.Path = audio.Path;
                audio_db.Name = audio.Name;
                audio_db.HotKey1 = audio.StrHotKey1;
                audio_db.HotKey2 = audio.StrHotKey2;
                audio_db.Volume = audio.Volume;
                _db.Audios.Update(audio_db);
                return _db.SaveChanges();
            }
            return 0;
        }

        public AudioDTO GetAudio(int id)
        {
            Audio? audio = _db.Audios.FirstOrDefault(x => x.Id == id);
            if (audio != null)
            {
                return ConvertToDTO(audio);
            }
            return new();
        }

        public List<AudioDTO> GetAllAudios()
        {
            var audios = _db.Audios.ToList();
            List<AudioDTO> audiosDTOs = new List<AudioDTO>();
            foreach (var audio in audios)
            {
                audiosDTOs.Add(ConvertToDTO(audio));
            }
            return audiosDTOs;
        }

        private AudioDTO ConvertToDTO(Audio audio)
        {
            return new()
            {
                Id = audio.Id,
                Path = audio.Path,
                Name = audio.Name,
                Volume = audio.Volume,
                HotKey1 = SD.hotkeysDictionary1.ContainsKey(audio.HotKey1) ? SD.hotkeysDictionary1[audio.HotKey1] : ModifierKeys.None,
                HotKey2 = SD.hotkeysDictionary2.ContainsKey(audio.HotKey2) ? SD.hotkeysDictionary2[audio.HotKey2] : Key.None,
                StrHotKey1 = audio.HotKey1,
                StrHotKey2 = audio.HotKey2,
            };
        }

        private Audio ConvertFromDTO(AudioDTO dto)
        {
            return new()
            {
                Id = dto.Id,
                Path = dto.Path,
                Name = dto.Name,
                Volume= dto.Volume,
                HotKey1 = dto.StrHotKey1,
                HotKey2 = dto.StrHotKey2,
            };
        }
    }
}
