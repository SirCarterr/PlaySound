using Microsoft.WindowsAPICodePack.Dialogs;
using PlaySound.Common;
using PlaySound.Model;
using PlaySound.Services;
using PlaySound.ViewModel.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PlaySound.ViewModel
{
    public class PlaySoundVM : INotifyPropertyChanged
    {
        //Class data
        private readonly AudioService _audioService;
        private readonly AudioManagerService _audioManagerService;

        public static string[] HotKeys1 => HotKeys.hotkeysDictionary1.Keys.ToArray();

        public static string[] HotKeys2 => HotKeys.hotkeysDictionary2.Keys.ToArray();

        //Binding properties
        public ObservableCollection<AudioDto> Audios { get; set; } = new();

        private AudioDto selectedAudio = new();

        public AudioDto SelectedAudio
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
            _audioService = new AudioService();
            _audioManagerService = new AudioManagerService();
            
            Task.FromResult(UpdateAudiosList());
            
            GetAudioFileCommand = new GetAudioFileCommand(this);
            StartEditCommand = new StartEditCommand(this);
            FinishEditingCommand = new FinishEditingCommand(this);
            CancelEditCommand = new CancelEditCommand(this);
            DeleteAudioCommand = new DeleteAudioCommand(this);
        }

        public async Task GetAudioFile()
        {
            var dialog = new CommonOpenFileDialog();
            
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var response = FileService.GetFileName(dialog.FileName);

                if (!response.IsSuccess)
                {
                    MessageBox.Show(response.Message, DialogCaptions.Error.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                    
                    return;
                }

                await _audioService.AddAudio(response.Audio!);
                
                await UpdateAudiosList();
            }
        }

        public void StartEditing()
        {
            IsEditing = true;

            IsEditingView = Visibility.Visible;
            IsMainView = Visibility.Hidden;

            SelectedAudio.IsEditEnabled = true;
        }

        public async Task FinishEditing()
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

        public async Task CancelEditing()
        {
            SelectedAudio.IsEditEnabled = false;
            IsEditing = false;

            await UpdateAudiosList();

            IsEditingView = Visibility.Hidden;
            IsMainView = Visibility.Visible;
        }

        public async Task DeleteAudio()
        {
            var Result = MessageBox.Show(DialogMessages.DeleteQuestion, DialogCaptions.Attention.ToString(), MessageBoxButton.YesNo, MessageBoxImage.Question);

            if(Result == MessageBoxResult.Yes)
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

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
