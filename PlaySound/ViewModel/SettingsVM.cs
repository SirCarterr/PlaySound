using Microsoft.WindowsAPICodePack.Dialogs;
using PlaySound.Model;
using PlaySound.ViewModel.Commands;
using PlaySound.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PlaySound.ViewModel
{
    public class SettingsVM : INotifyPropertyChanged
    {
        private readonly string[] keys = new string[] { "backspace", "delete", "tab", "clear", "return", "pause", "escape", "space", "up", "down", "right", "left", "insert", "home", "end", "page up", "page down", "f1", "f2", "f3", "f4", "f5", "f6", "f7", "f8", "f9", "f10", "f11", "f12", "f13", "f14", "f15", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "!", "\"", "#", "$", "&", "'", "(", ")", "*", "+", ",", "-", ".", "/", ":", ";", "<", "=", ">", "?", "@", "[", "\\", "]", "^", "_", "`", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "numlock", "caps lock", "scroll lock" };

        public string[] Keys 
        { 
            get { return keys; } 
        }

        public int PlayKeyIndex { get; set; }
        public int StopKeyIndex { get; set; }
        public int NextKeyIndex { get; set; }
        public int PreviousKeyIndex { get; set; }

        private Settings settings = new();

        public Settings Settings
        {
            get { return settings; }
            set 
            { 
                settings = value;
                OnPropertyChanged(nameof(Settings));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public GetDirectoryCommand GetDirectoryCommand { get; set; }
        public SaveSettingsCommand SaveSettingsCommand { get; set; }

        public SettingsVM()
        {
            Settings = SettingsHelper.GetSettings();

            if (!string.IsNullOrEmpty(Settings.PlayAudioKey))
                PlayKeyIndex = Array.IndexOf(Keys, Settings.PlayAudioKey);
            if (!string.IsNullOrEmpty(Settings.StopAudioKey))
                StopKeyIndex = Array.IndexOf(Keys, Settings.StopAudioKey);
            if (!string.IsNullOrEmpty(Settings.NextAudioKey))
                NextKeyIndex = Array.IndexOf(Keys, Settings.NextAudioKey);
            if (!string.IsNullOrEmpty(Settings.PreviousAudioKey))
                PreviousKeyIndex = Array.IndexOf(Keys, Settings.PreviousAudioKey);

            GetDirectoryCommand = new GetDirectoryCommand(this);
            SaveSettingsCommand = new SaveSettingsCommand(this);
        }

        public void GetAudioDirectory()
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Settings = new Settings
                {
                    DirectoryPath = dialog.FileName,
                    PlayAudioKey = settings.PlayAudioKey,
                    StopAudioKey = settings.StopAudioKey,
                    NextAudioKey = settings.NextAudioKey,
                    PreviousAudioKey = settings.PreviousAudioKey
                };
            }
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => !w.IsActive && w.Name.Equals("SettingsPlaySound"));
            window?.Focus();
        }

        public void SaveSettings()
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive && w.Name.Equals("SettingsPlaySound"));
            if (window != null)
            {
                SettingsHelper.UpdateSettings(Settings);
                window.DialogResult = true;
                window.Close();
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
