using PlaySound.Common;
using PlaySound.Model;
using System.Windows.Input;

namespace PlaySound.Converter
{
    public static class AudioConverter
    {
        public static AudioDto ConvertToDTO(Audio audio)
        {
            return new()
            {
                Id = audio.Id,
                Path = audio.Path,
                Name = audio.Name,
                Volume = audio.Volume,
                HotKey1 = HotKeys.hotkeysDictionary1.ContainsKey(audio.HotKey1) ? HotKeys.hotkeysDictionary1[audio.HotKey1] : ModifierKeys.None,
                HotKey2 = HotKeys.hotkeysDictionary2.ContainsKey(audio.HotKey2) ? HotKeys.hotkeysDictionary2[audio.HotKey2] : Key.None,
                StrHotKey1 = audio.HotKey1,
                StrHotKey2 = audio.HotKey2,
            };
        }

        public static Audio ConvertFromDTO(AudioDto dto)
        {
            return new()
            {
                Id = dto.Id,
                Path = dto.Path,
                Name = dto.Name,
                Volume = dto.Volume,
                HotKey1 = dto.StrHotKey1,
                HotKey2 = dto.StrHotKey2,
            };
        }
    }
}
