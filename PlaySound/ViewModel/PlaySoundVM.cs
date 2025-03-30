using Microsoft.WindowsAPICodePack.Dialogs;
using PlaySound.Common;
using PlaySound.Interfaces;
using PlaySound.Model;
using PlaySound.Services;
using PlaySound.ViewModel.Commands;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PlaySound.ViewModel
{
    public class PlaySoundVM : BaseViewModel
    {
        private readonly IAudioService _audioService;
        private readonly IAudioManagerService _audioManagerService;
        private readonly IDialogService _dialogService;

        public static string[] HotKeys1 => HotKeys.hotkeysDictionary1.Keys.ToArray();
        public static string[] HotKeys2 => HotKeys.hotkeysDictionary2.Keys.ToArray();

        public ObservableCollection<AudioDto> Audios { get; set; } = new();

        private AudioDto selectedAudio = new();

        public AudioDto SelectedAudio
        {
            get => selectedAudio;
            set => SetProperty(ref selectedAudio, value);
        }

        private bool isEditing = false;

        public bool IsEditing
        {
            get => isEditing;
            set => SetProperty(ref isEditing, value);
        }

        private Visibility isEditingView = Visibility.Hidden;

        public Visibility IsEditingView 
        {
            get => isEditingView;
            set => SetProperty(ref isEditingView, value);
        }

        private Visibility isMainView = Visibility.Visible;

        public Visibility IsMainView
        {
            get => isMainView;
            set => SetProperty(ref isMainView, value);
        }

        public ICommand GetAudioFileCommand { get; }
        public ICommand StartEditCommand { get; }
        public ICommand FinishEditingCommand { get; }
        public ICommand CancelEditCommand { get; }
        public ICommand DeleteAudioCommand { get; }

        public PlaySoundVM(
            IAudioService audioService, 
            IAudioManagerService audioManagerService,
            IDialogService dialogService)
        {
            _audioService = audioService;
            _audioManagerService = audioManagerService;
            _dialogService = dialogService;
            
            Task.FromResult(UpdateAudiosList());
            
            GetAudioFileCommand = new AsyncRelayCommand(_ => GetAudioFile());
            StartEditCommand = new AsyncRelayCommand(_ => { StartEditing(); return Task.CompletedTask; }, _ => SelectedAudio?.Id != 0);
            FinishEditingCommand = new AsyncRelayCommand(_ => FinishEditing());
            CancelEditCommand = new AsyncRelayCommand(_ => CancelEditing());
            DeleteAudioCommand = new AsyncRelayCommand(_ => DeleteAudio());
        }

        private async Task GetAudioFile()
        {
            var fileName = await _dialogService.OpenFileDialog("Audio files (*.mp3)|*.mp3");
            if (fileName != null)
            {
                var response = FileService.GetFileName(fileName);

                if (!response.IsSuccess)
                {
                    _dialogService.ShowError(response.Message, DialogCaptions.Error.ToString());
                    return;
                }

                await _audioService.AddAudio(response.Audio!);
                
                await UpdateAudiosList();
            }
        }

        private void StartEditing()
        {
            IsEditing = true;

            IsEditingView = Visibility.Visible;
            IsMainView = Visibility.Hidden;

            SelectedAudio.IsEditEnabled = true;
        }

        private async Task FinishEditing()
        {
            SelectedAudio.IsEditEnabled = false;
            IsEditing = false;

            AudioDto audio = new()
            {
                Id = SelectedAudio.Id,
                Path = SelectedAudio.Path,
                Name = SelectedAudio.Name,
                StrHotKey1 = SelectedAudio.StrHotKey1,
                StrHotKey2 = SelectedAudio.StrHotKey2,
                Volume = SelectedAudio.Volume
            };

            await _audioService.UpdateAudio(audio);
            
            await UpdateAudiosList();
            
            IsEditingView = Visibility.Hidden;
            IsMainView = Visibility.Visible;
        }

        private async Task CancelEditing()
        {
            SelectedAudio.IsEditEnabled = false;
            IsEditing = false;

            await UpdateAudiosList();

            IsEditingView = Visibility.Hidden;
            IsMainView = Visibility.Visible;
        }

        private async Task DeleteAudio()
        {
            var confirmed = await _dialogService.ShowConfirmationDialog(
                DialogMessages.DeleteQuestion, 
                DialogCaptions.Attention.ToString());

            if(confirmed)
            {
                SelectedAudio.IsEditEnabled = false;
                IsEditing = false;
                
                await _audioService.RemoveAudio(SelectedAudio.Id);
                
                await UpdateAudiosList();
                
                IsEditingView = Visibility.Hidden;
                IsMainView = Visibility.Visible;
            }
        }

        private async Task UpdateAudiosList()
        {
            SelectedAudio = new();
            Audios.Clear();

            var audios = await _audioService.GetAllAudios();

            _audioManagerService.InitializeHotKeys(audios);

            foreach (var audio in audios)
            {
                Audios.Add(audio);
            }
        }
    }
}
