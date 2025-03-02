using PlaySound.Common;
using PlaySound.Model;
using System.IO;
using System.Linq;

namespace PlaySound.Services
{
    public static class FileService
    {
        private const string ValidExtension = ".mp3";
        private const int MaxFileSize = 3000000;

        private const string DefaultHotKetName = "None";
        private const float DefaultVolume = 1.0f;

        public static DialogResponseDto GetFileName(string fileName)
        {
            FileInfo fileInfo = new(fileName);
            
            if (fileInfo.Extension != ValidExtension)
            {
                return new DialogResponseDto
                {
                    IsSuccess = false,
                    Message = DialogMessages.InvalidFileExtension
                };
            }
            
            if (fileInfo.Length > MaxFileSize)
            {
                return new DialogResponseDto
                {
                    IsSuccess = false,
                    Message = DialogMessages.FileSizeExceed
                };
            }

            AudioDto audioDto = new()
            {
                Path = fileName,
                Name = fileName.Split('\\').Last(),
                StrHotKey1 = DefaultHotKetName,
                StrHotKey2 = DefaultHotKetName,
                Volume = DefaultVolume
            };

            return new DialogResponseDto
            {
                IsSuccess = true,
                Audio = audioDto
            };
        }
    }
}
