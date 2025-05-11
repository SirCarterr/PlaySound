using Microsoft.WindowsAPICodePack.Dialogs;
using PlaySound.Interfaces;
using System.Threading.Tasks;
using System.Windows;

namespace PlaySound.Services
{
    public class DialogService : IDialogService
    {
        public Task<string?> OpenFileDialog(string filter = "All files (*.*)|*.*")
        {
            return Task.Run(() =>
            {
                var dialog = new CommonOpenFileDialog();
                return Application.Current.Dispatcher.Invoke(() =>
                {
                    if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                    {
                        return dialog.FileName;
                    }
                    return null;
                });
            });
        }

        public Task<bool> ShowConfirmationDialog(string message, string title)
        {
            return Task.Run(() =>
            {
                return Application.Current.Dispatcher.Invoke(() =>
                {
                    var result = MessageBox.Show(
                        message,
                        title,
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);
                    return result == MessageBoxResult.Yes;
                });
            });
        }

        public void ShowError(string message, string title, MessageBoxImage icon = MessageBoxImage.Error)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(
                    message,
                    title,
                    MessageBoxButton.OK,
                    icon);
            });
        }

        public void ShowInformation(string message, string title, MessageBoxImage icon = MessageBoxImage.Information)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(
                    message,
                    title,
                    MessageBoxButton.OK,
                    icon);
            });
        }
    }
} 