using PlaySound.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaySound.ViewModel.Helpers
{
    public static class AudioManager
    {
        private static readonly string path = "";

        public static ResponseModel GetAudios()
        {
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);
                List<string> audios = new();
                foreach (var file in files)
                {
                    FileInfo fileInfo = new(Path.Combine(path, file));
                    if (fileInfo.Extension.Equals(".mp3") && fileInfo.Length < 5000000)
                    {
                        audios.Add(fileInfo.Name);
                    }
                }

                return new ResponseModel()
                {
                    Data = audios.ToArray(),
                    IsError = false
                };
            }
            return new ResponseModel()
            {
                IsError = true,
                Error = "Directory with audio is missing!"
            };
        }
    }
}
