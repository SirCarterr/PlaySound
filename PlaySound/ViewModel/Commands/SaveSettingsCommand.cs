using PlaySound.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlaySound.ViewModel.Commands
{
    public class SaveSettingsCommand : ICommand
    {
        private SettingsVM SettingsVM { get; set; }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public SaveSettingsCommand(SettingsVM settingsVM)
        {
            SettingsVM = settingsVM;
        }

        public bool CanExecute(object? parameter)
        {
            Settings? settings = parameter as Settings;

            if (settings == null)
                return false;

            if (!SettingsVM.Keys.Contains(settings.PlayAudioKey)) 
                return false;
            if (!SettingsVM.Keys.Contains(settings.StopAudioKey))
                return false;
            if (!SettingsVM.Keys.Contains(settings.NextAudioKey))
                return false;
            if (!SettingsVM.Keys.Contains(settings.PreviousAudioKey))
                return false;

            return true;
        }

        public void Execute(object? parameter)
        {
            SettingsVM.SaveSettings();
        }
    }
}
