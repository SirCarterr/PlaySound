using PlaySound.Model;
using PlaySound.View;
using PlaySound.ViewModel.Commands;
using PlaySound.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaySound.ViewModel
{
    public class PlaySoundVM : INotifyPropertyChanged
    {
        private Settings settings = new();

        public Settings Settings
        {
            get { return settings; }
            set 
            { 
                settings = value;
                OnPropertyChanged(nameof(settings));
            }
        }

        private string[]? audios;

        public string[]? Audios
        {
            get { return audios; }
            set 
            { 
                audios = value;
                OnPropertyChanged(nameof(Audios));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public OpenSettingsCommand OpenSettingsCommand { get; set; }

        public PlaySoundVM()
        {
            Settings = SettingsHelper.GetSettings();

            OpenSettingsCommand = new OpenSettingsCommand(this);
        }

        public void OpenSettingsWindow()
        {
            var settingsWindow = new SettingsWindow();
            bool? result = settingsWindow.ShowDialog();
            if (result == null)
                return;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
