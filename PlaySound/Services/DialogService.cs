using Microsoft.WindowsAPICodePack.Dialogs;
using PlaySound.Interfaces;
using System;
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
                return InvokeOnUIThread(() =>
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
                return InvokeOnUIThread(() =>
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
            ShowMessageBox(message, title, MessageBoxButton.OK, icon);
        }

        public void ShowInformation(string message, string title, MessageBoxImage icon = MessageBoxImage.Information)
        {
            ShowMessageBox(message, title, MessageBoxButton.OK, icon);
        }

        private void ShowMessageBox(string message, string title, MessageBoxButton buttons, MessageBoxImage icon)
        {
            InvokeOnUIThread(() =>
            {
                MessageBox.Show(
                    message,
                    title,
                    buttons,
                    icon);
            });
        }

        private T InvokeOnUIThread<T>(Func<T> action)
        {
            return Application.Current.Dispatcher.Invoke(action);
        }

        private void InvokeOnUIThread(Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }
    }
} 