using PlaySound.Common;
using PlaySound.Constants;
using PlaySound.Interfaces;
using PlaySound.Model;
using System.IO;
using System.Linq;

namespace PlaySound.Services
{
    public class FileService
    {
        private readonly IConfigurationService _configurationService;

        public FileService(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public DialogResponseDto GetFileName(string fileName)
        {
            FileInfo fileInfo = new(fileName);
            
            if (fileInfo.Extension != _configurationService.ValidAudioExtension)
            {
                return new DialogResponseDto
                {
                    IsSuccess = false,
                    Message = DialogMessages.InvalidFileExtension
                };
            }
            
            if (fileInfo.Length > _configurationService.MaxFileSize)
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
                StrHotKey1 = _configurationService.DefaultHotKeyName,
                StrHotKey2 = _configurationService.DefaultHotKeyName,
                Volume = _configurationService.DefaultVolume
            };

            return new DialogResponseDto
            {
                IsSuccess = true,
                Audio = audioDto
            };
        }
    }
}
