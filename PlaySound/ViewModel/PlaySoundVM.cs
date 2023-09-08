using Microsoft.WindowsAPICodePack.Dialogs;
using PlaySound.Common;
using PlaySound.Model;
using PlaySound.View;
using PlaySound.ViewModel.Commands;
using PlaySound.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlaySound.ViewModel
{
    public class PlaySoundVM : INotifyPropertyChanged
    {
        //Class data
        private readonly AudioManager _audioManager;

        public string[] HotKeys1
        { 
            get 
            {
                return SD.hotkeys1;
            } 
        }
        
        public string[] HotKeys2
        { 
            get 
            {
                return SD.hotkeys2;
            } 
        }

        //Binding properties
        public ObservableCollection<Audio> Audios { get; set; } = new();

        private Audio selectedAudio = new();

        public Audio SelectedAudio
        {
            get { return selectedAudio; }
            set
            {
                selectedAudio = value;
                OnPropertyChanged(nameof(Audio));
            }
        }

        private bool isEditing = false;

        public bool IsEditing
        {
            get { return isEditing; }
            set 
            { 
                isEditing = value; 
                OnPropertyChanged(nameof(IsEditing));
            }
        }

        public Visibility IsEditingView 
        {
            get 
            {
                return isEditing ? Visibility.Visible : Visibility.Hidden;
            } 
        }

        public Visibility IsMainView
        {
            get
            {
                return !isEditing ? Visibility.Visible : Visibility.Hidden;
            }
        }

        //Commands
        public GetAudioFileCommand GetAudioFileCommand { get; set; }
        public StartEditCommand StartEditCommand { get; set; }
        public FinishEditingCommand FinishEditingCommand { get; set; }
        public CancelEditCommand CancelEditCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public PlaySoundVM()
        {
            _audioManager = new AudioManager();
            UpdateAudiosList();

            GetAudioFileCommand = new GetAudioFileCommand(this);
            StartEditCommand = new StartEditCommand(this);
            FinishEditingCommand = new FinishEditingCommand(this);
            CancelEditCommand = new CancelEditCommand(this);
        }

        public void GetAudioFile()
        {
            var dialog = new CommonOpenFileDialog();
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                if (dialog.FileName.Split('.').Last().Equals("mp3"))
                {
                    Audio audio = new()
                    {
                        Path = dialog.FileName,
                        Name = dialog.FileName.Split('\\').Last(),
                        HotKey1 = "left ctrl",
                        HotKey2 = "1",
                    };
                    _audioManager.AddAudio(audio);
                    UpdateAudiosList();
                    //_audioManager.RemoveAudio(1);
                }
                else
                {
                    MessageBox.Show("Invalid file extension", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            //var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => !w.IsActive && w.Name.Equals("SettingsPlaySound"));
            //window?.Focus();
        }

        public void StartEditing()
        {
            IsEditing = true;
            SelectedAudio.IsEditEnabled = true;
        }

        public void FinishEditing()
        {
            SelectedAudio.IsEditEnabled = false;
            IsEditing = false;

            Audio audio = new()
            {
                Id = SelectedAudio.Id,
                Path = SelectedAudio.Path,
                Name = SelectedAudio.Name,
                HotKey1 = SelectedAudio.HotKey1,
                HotKey2 = SelectedAudio.HotKey2,
            };

            _audioManager.UpdateAudio(audio);
            UpdateAudiosList();
        }

        public void CancelEditing()
        {
            SelectedAudio.IsEditEnabled = false;
            IsEditing = false;
            UpdateAudiosList();
        }

        private void UpdateAudiosList()
        {
            Audios.Clear();
            var audios = _audioManager.GetAllAudios();
            foreach (var audio in audios)
            {
                Audios.Add(audio);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
