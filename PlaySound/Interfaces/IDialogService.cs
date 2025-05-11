using System.Threading.Tasks;
using System.Windows;

namespace PlaySound.Interfaces
{
    public interface IDialogService
    {
        Task<string?> OpenFileDialog(string filter = "All files (*.*)|*.*");
        Task<bool> ShowConfirmationDialog(string message, string title);
        void ShowError(string message, string title, MessageBoxImage icon = MessageBoxImage.Error);
        void ShowInformation(string message, string title, MessageBoxImage icon = MessageBoxImage.Information);
    }
} 