using System.Threading.Tasks;

namespace PlaySound.Interfaces
{
    public interface IDialogService
    {
        Task<string?> OpenFileDialog(string filter = "All files (*.*)|*.*");
        Task<bool> ShowConfirmationDialog(string message, string title);
        void ShowError(string message, string title);
        void ShowInformation(string message, string title);
    }
} 