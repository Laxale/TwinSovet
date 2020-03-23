using System;
using System.Windows;
using System.Windows.Threading;

using Microsoft.Practices.Unity;

using NLog;
using TwinSovet.Helpers;

using Prism.Mvvm;

using PubSub;

using TwinSovet.Messages;
using TwinSovet.ViewModels;


namespace TwinSovet 
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly SingleInstancesCache instancesCache;
        private readonly ILogger logger = LogManager.GetCurrentClassLogger();
        private readonly ViewMappingCache viewMappingCache = new ViewMappingCache();


        public App() 
        {
            instancesCache = new SingleInstancesCache(type => MainContainer.Instance.Resolve(type));
        }


        /// <summary>
        ///   Вызывает событие <see cref="E:System.Windows.Application.Startup" />.
        /// </summary>
        /// <param name="e">
        ///   Объект <see cref="T:System.Windows.StartupEventArgs" />, содержащий данные события.
        /// </param>
        protected override void OnStartup(StartupEventArgs e) 
        {
            DispatcherUnhandledException += OnDispatcherUnhandledException;

            base.OnStartup(e);

            var initer = ViewModelInitializer.Instance;

            ViewModelLocationProvider.SetDefaultViewModelFactory(instancesCache.GetOrCreateInstance);
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(viewMappingCache.GetViewModelType);
        }


        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) 
        {
            string message = $"Произошла ошибка.{Environment.NewLine}{e.Exception.GetRootException().Message}";

            logger.Error(message);

            MessageBox.Show(message);

            Environment.Exit(-6);
        }


        private void SimpleFlatView_OnEventShowFlatDetails(FlatInListDecoratorViewModel flat) 
        {
            this.Publish(new MessageShowFlatDetails(flat));
        }
    }
}