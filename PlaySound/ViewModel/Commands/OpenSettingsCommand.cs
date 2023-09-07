using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlaySound.ViewModel.Commands
{
    public class OpenSettingsCommand : ICommand
    {
        private PlaySoundVM PlaySoundVM { get; set; }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public OpenSettingsCommand(PlaySoundVM playSoundVM)
        {
            PlaySoundVM = playSoundVM;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            PlaySoundVM.OpenSettingsWindow();
        }
    }
}
