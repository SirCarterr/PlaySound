using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaySound.ViewModel
{
    public class PlaySoundVM : INotifyPropertyChanged
    {
        private string[]? audios;

        public string[]? Audios
        {
            get { return audios; }
            set 
            { 
                audios = value;
                OnPropertyChanged(nameof(Audios));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
