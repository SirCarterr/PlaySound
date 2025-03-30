using Microsoft.Extensions.DependencyInjection;
using System;

namespace PlaySound.ViewModel
{
    public class ViewModelLocator
    {
        private static readonly IServiceProvider _serviceProvider;

        static ViewModelLocator()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            // Register all ViewModels
            services.AddSingleton<PlaySoundVM>();
        }

        public PlaySoundVM PlaySoundViewModel => _serviceProvider.GetRequiredService<PlaySoundVM>();
    }
} 