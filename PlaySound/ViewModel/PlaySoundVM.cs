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
using System.IO;
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

        public static string[] HotKeys1
        { 
            get 
            {
                return SD.hotkeys1;
            } 
        }
        
        public static string[] HotKeys2
        { 
            get 
            {
                return SD.hotkeys2;
            } 
        }

        //Binding properties
        public ObservableCollection<AudioDTO> Audios { get; set; } = new();

        private AudioDTO selectedAudio = new();

        public AudioDTO SelectedAudio
        {
            get { return selectedAudio; }
            set
            {
                selectedAudio = value;
                OnPropertyChanged(nameof(SelectedAudio));
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

        private Visibility isEditingView = Visibility.Hidden;

        public Visibility IsEditingView 
        {
            get 
            {
                return isEditingView;
            } 
            set
            {
                isEditingView = value;
                OnPropertyChanged(nameof(IsEditingView));
            }
        }

        private Visibility isMainView = Visibility.Visible;

        public Visibility IsMainView
        {
            get
            {
                return isMainView;
            }
            set
            {
                isMainView = value;
                OnPropertyChanged(nameof(IsMainView));
            }
        }

        //Commands
        public GetAudioFileCommand GetAudioFileCommand { get; set; }
        public StartEditCommand StartEditCommand { get; set; }
        public FinishEditingCommand FinishEditingCommand { get; set; }
        public CancelEditCommand CancelEditCommand { get; set; }
        public DeleteAudioCommand DeleteAudioCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public PlaySoundVM()
        {
            _audioManager = new AudioManager();
            UpdateAudiosList();

            GetAudioFileCommand = new GetAudioFileCommand(this);
            StartEditCommand = new StartEditCommand(this);
            FinishEditingCommand = new FinishEditingCommand(this);
            CancelEditCommand = new CancelEditCommand(this);
            DeleteAudioCommand = new DeleteAudioCommand(this);
        }

        public void GetAudioFile()
        {
            var dialog = new CommonOpenFileDialog();
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                FileInfo fileInfo = new(dialog.FileName);
                if (fileInfo.Extension != ".mp3")
                {
                    MessageBox.Show("Invalid file extension!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if(fileInfo.Length > 3000000)
                {
                    MessageBox.Show("Chosen file size exceed 3Mb!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                AudioDTO audio = new()
                {
                    Path = dialog.FileName,
                    Name = dialog.FileName.Split('\\').Last(),
                    HotKey1 = "left ctrl",
                    HotKey2 = "1",
                };
                _audioManager.AddAudio(audio);
                UpdateAudiosList();
            }
            //var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => !w.IsActive && w.Name.Equals("SettingsPlaySound"));
            //window?.Focus();
        }

        public void StartEditing()
        {
            IsEditing = true;
            IsEditingView = Visibility.Visible;
            IsMainView = Visibility.Hidden;
            SelectedAudio.IsEditEnabled = true;
        }

        public void FinishEditing()
        {
            SelectedAudio.IsEditEnabled = false;
            IsEditing = false;
            IsEditingView = Visibility.Hidden;
            IsMainView = Visibility.Visible;

            AudioDTO audio = new()
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
            IsEditingView = Visibility.Hidden;
            IsMainView = Visibility.Visible;
            UpdateAudiosList();
        }

        public void DeleteAudio()
        {
            var Result = MessageBox.Show("Do you want to remove this audio?", "Attention", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if(Result == MessageBoxResult.Yes)
            {
                SelectedAudio.IsEditEnabled = false;
                IsEditing = false;
                IsEditingView = Visibility.Hidden;
                IsMainView = Visibility.Visible;
                _audioManager.RemoveAudio(SelectedAudio.Id);
                UpdateAudiosList();
            }
        }

        private void UpdateAudiosList()
        {
            SelectedAudio = new();
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
