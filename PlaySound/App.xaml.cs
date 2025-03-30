using Microsoft.Extensions.DependencyInjection;
using PlaySound.Interfaces;
using PlaySound.Services;
using PlaySound.ViewModel;
using System;
using System.Windows;

namespace PlaySound
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            // Register services
            services.AddSingleton<IAudioService, AudioService>();
            services.AddSingleton<IAudioManagerService, AudioManagerService>();
            services.AddSingleton<IDialogService, DialogService>();

            // Register ViewModels
            services.AddSingleton<PlaySoundVM>();

            // Register Views
            services.AddTransient<View.PlaySoundWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = _serviceProvider.GetRequiredService<View.PlaySoundWindow>();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
