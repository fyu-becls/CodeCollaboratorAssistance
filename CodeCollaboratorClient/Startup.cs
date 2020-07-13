using Authoring.Infrastructure.ServiceLocator;
using Autofac.Core;
using CodeCollaboratorClient.ViewModels;
using CollabCommandAPI;
using CollabCommandAPI.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using CodeCollaboratorClient.Authentication;
using CodeCollaboratorClient.CollabAPIManager;

namespace CodeCollaboratorClient
{
    internal static class Startup
    {
        /// <summary>
        /// The DI service collection.
        /// </summary>
        private static readonly ServiceCollection ServiceCollection = new ServiceCollection();

        /// <summary>
        /// Configure the software at startup.
        /// </summary>
        public static void Configure()
        {
            ConfigureServices();
            LoadUserSettings();
        }

        private static void LoadUserSettings()
        {
            var settings = ServiceLocatorManager.Instance.GlobalServiceLocator.GetService<CollabConnectionSettings>();
            settings.ServerURL = UserSettings.Default.ServerUrl;
            settings.UserName = UserSettings.Default.UserName;
            settings.UserPassword = UserSettings.Default.UserPassword;
        }

        /// <summary>
        /// Configure DI services.
        /// </summary>
        private static void ConfigureServices()
        {
            ServiceCollection.Clear();

            // Add global services.
            ServiceCollection.AddSingleton<AuthenticationCommands>();
            ServiceCollection.AddSingleton<CommandsService>();
            ServiceCollection.AddSingleton<ReviewCommands>();
            ServiceCollection.AddSingleton<ICollabServerConnection, CollabServerConnection>();
            ServiceCollection.AddSingleton<ICollabReviewService, CollabReviewService>();
            ServiceCollection.AddSingleton<ICollabCommandExecutor, CollabServerConnection>();
            ServiceCollection.AddSingleton<CollabConnectionSettings>(); 
            ServiceCollection.AddSingleton<LoginViewModel>();
            ServiceCollection.AddSingleton<IAuthenticationService, AuthenticationService>();
            ServiceCollection.AddSingleton<APIManager>();
            ServiceCollection.AddSingleton<HomeViewModel>();
            ServiceCollection.AddSingleton<GlobalParameters>();
            ServiceCollection.AddTransient<ReviewViewModel>();

            ServiceLocatorManager.Instance.Initialize(ServiceCollection.BuildServiceProvider());
        }
    }
}
