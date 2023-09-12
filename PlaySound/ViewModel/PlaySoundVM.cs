using Microsoft.WindowsAPICodePack.Dialogs;
using NAudio.CoreAudioApi;
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
        private readonly AudioPlaybackService _audioPlaybackService;
        private readonly GlobalHotKeyService _globalHotKeyService;

        private List<CachedSound> _soundsVB = new List<CachedSound>();
        private List<CachedSound> _soundsDefault = new List<CachedSound>();

        public static string[] HotKeys1
        { 
            get 
            {
                return SD.hotkeysDictionary1.Keys.ToArray();
            } 
        }
        
        public static string[] HotKeys2
        { 
            get 
            {
                return SD.hotkeysDictionary2.Keys.ToArray();
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
            _globalHotKeyService = new GlobalHotKeyService();

            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            MMDevice defaultPlaybackDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            int outRate = defaultPlaybackDevice.AudioClient.MixFormat.SampleRate;
            _audioPlaybackService = new AudioPlaybackService(outRate);
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
                    StrHotKey1 = "None",
                    StrHotKey2 = "None",
                    Volume = 1.0f
                };
                _audioManager.AddAudio(audio);
                UpdateAudiosList();
            }
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

            AudioDTO audio = new()
            {
                Id = SelectedAudio.Id,
                Path = SelectedAudio.Path,
                Name = SelectedAudio.Name,
                StrHotKey1 = SelectedAudio.StrHotKey1,
                StrHotKey2 = SelectedAudio.StrHotKey2,
                Volume = SelectedAudio.Volume
            };

            _audioManager.UpdateAudio(audio);
            UpdateAudiosList();
            
            IsEditingView = Visibility.Hidden;
            IsMainView = Visibility.Visible;
        }

        public void CancelEditing()
        {
            SelectedAudio.IsEditEnabled = false;
            IsEditing = false;
            UpdateAudiosList();
            IsEditingView = Visibility.Hidden;
            IsMainView = Visibility.Visible;
        }

        public void DeleteAudio()
        {
            var Result = MessageBox.Show("Do you want to remove this audio?", "Attention", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if(Result == MessageBoxResult.Yes)
            {
                SelectedAudio.IsEditEnabled = false;
                IsEditing = false;
                _audioManager.RemoveAudio(SelectedAudio.Id);
                UpdateAudiosList();
                IsEditingView = Visibility.Hidden;
                IsMainView = Visibility.Visible;
            }
        }

        private void UpdateAudiosList()
        {
            SelectedAudio = new();
            Audios.Clear();
            _soundsVB.Clear();
            _soundsDefault.Clear();
            _globalHotKeyService.UnregisterAllHotkeys();
            var audios = _audioManager.GetAllAudios();
            foreach (var audio in audios)
            {
                if (!File.Exists(audio.Path))
                    continue;

                Audios.Add(audio);
                _soundsVB.Add(new CachedSound(audio.Id, audio.Volume, audio.Path));
                _soundsDefault.Add(new CachedSound(audio.Id, audio.Volume, audio.Path));
                _globalHotKeyService.RegisterHotkey(audio.HotKey1, audio.HotKey2, () => PlayAudio(audio.Id));
            }
        }

        private void PlayAudio(int Id)
        {
            CachedSound soundVB = _soundsVB.First(s => s.Id == Id);
            CachedSound soundDefault = _soundsDefault.First(s => s.Id == Id);
            _audioPlaybackService.PlaySoundVB(soundVB);
            _audioPlaybackService.PlaySoundDefault(soundDefault);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
