using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlaySound.ViewModel.Commands
{
    public class PlayAudioCommand : ICommand
    {
        private readonly PlaySoundVM playSoundVM;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public PlayAudioCommand(PlaySoundVM playSoundVM)
        {
            this.playSoundVM = playSoundVM;
        }

        public bool CanExecute(object? parameter)
        {
            string? p = parameter as string;
            
            if(string.IsNullOrEmpty(p))
                return false;

            return true;
        }

        public void Execute(object? parameter)
        {
            string? p = parameter as string;
            playSoundVM.PlayAudio(p!);
        }
    }
}
