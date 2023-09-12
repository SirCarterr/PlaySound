using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlaySound.Model
{
    public class AudioDTO : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public ModifierKeys HotKey1 { get; set; }
        public string StrHotKey1 { get; set; }
        public Key HotKey2 { get; set; }
        public string StrHotKey2 { get; set; }
        public float Volume { get; set; }

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
