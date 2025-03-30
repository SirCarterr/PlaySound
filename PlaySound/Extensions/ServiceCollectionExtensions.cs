using Microsoft.Extensions.DependencyInjection;
using PlaySound.Interfaces;
using PlaySound.Services;
using PlaySound.ViewModel;

namespace PlaySound.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IConfigurationService, ConfigurationService>();
            services.AddSingleton<IAudioService, AudioService>();
            services.AddSingleton<IAudioManagerService, AudioManagerService>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<FileService>();

            return services;
        }

        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            services.AddSingleton<PlaySoundVM>();

            return services;
        }

        public static IServiceCollection AddViews(this IServiceCollection services)
        {
            services.AddTransient<View.PlaySoundWindow>();

            return services;
        }
    }
} 