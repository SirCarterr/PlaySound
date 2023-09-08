using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlaySound.ViewModel.Commands
{
    public class GetAudioFileCommand : ICommand
    {
        private readonly PlaySoundVM playSoundVM;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public GetAudioFileCommand(PlaySoundVM playSoundVM)
        {

            this.playSoundVM = playSoundVM;

        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
           playSoundVM.GetAudioFile();
        }
    }
}
