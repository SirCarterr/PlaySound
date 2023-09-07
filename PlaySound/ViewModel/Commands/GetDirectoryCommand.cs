using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlaySound.ViewModel.Commands
{
    public class GetDirectoryCommand : ICommand
    {
        private SettingsVM SettingsVM { get; set; }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public GetDirectoryCommand(SettingsVM settingsVM)
        {
            SettingsVM = settingsVM;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            SettingsVM.GetAudioDirectory();
        }
    }
}
