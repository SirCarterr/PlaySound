using PlaySound.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlaySound.ViewModel.Commands
{
    public class StartEditCommand : ICommand
    {
        private readonly PlaySoundVM playSoundVM;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public StartEditCommand(PlaySoundVM playSoundVM)
        {
            this.playSoundVM = playSoundVM;
        }

        public bool CanExecute(object? parameter)
        {
            return parameter != null && (parameter as AudioDto)?.Id != 0;
        }

        public void Execute(object? parameter)
        {
            playSoundVM.StartEditing();
        }
    }
}
