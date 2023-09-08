using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaySound.Model
{
    public class AudioDTO : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public string HotKey1 { get; set; }
        public string HotKey2 { get; set; }

        private bool isEditEnabled = false;

        public bool IsEditEnabled 
        { 
            get 
            {
                return isEditEnabled;
            } 
            set
            {
                isEditEnabled = value;
                OnPropertyChanged(nameof(IsEditEnabled));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
