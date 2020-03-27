using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

using TwinSovet.Helpers;
using TwinSovet.Data.Enums;
using TwinSovet.Data.Helpers;
using TwinSovet.Extensions;
using TwinSovet.Interfaces;
using TwinSovet.Messages;
using TwinSovet.Messages.Attachments;
using TwinSovet.Providers;
using TwinSovet.ViewModels;
using TwinSovet.Views;
using TwinSovet.Messages.Details;
using TwinSovet.Messages.Indications;
using TwinSovet.Data.DataBase;
using TwinSovet.Data.DataBase.Interfaces;
using TwinSovet.ViewModels.Attachments;

using NLog;

using PubSub;

using Prism.Mvvm;

using Microsoft.Practices.Unity;

using LocRes = TwinSovet.Localization.Resources;


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
        /// Вызывает событие <see cref="Application.Startup" />.
        /// </summary>
        /// <param name="e">Объект <see cref="StartupEventArgs" />, содержащий данные события.</param>
        protected override void OnStartup(StartupEventArgs e) 
        {
            DispatcherUnhandledException += OnDispatcherUnhandledException;

            DbValidator.VerifyDatabase();

            base.OnStartup(e);

            var initer = ViewModelInitializer.Instance;

            MainContainer.Instance.RegisterType<IDbContextFactory, DbContextFactory>();
            MainContainer.Instance.RegisterInstance<IUnityContainer>(MainContainer.Instance);
            MainContainer.Instance.RegisterInstance<AllFloorsProvider>(AllFloorsProvider.Instance);
            MainContainer.Instance.RegisterInstance<IFloorsProvider>(nameof(SectionType.Furniture), AllFloorsProvider.Instance.FurnitureFloorsProvider);
            MainContainer.Instance.RegisterInstance<IFloorsProvider>(nameof(SectionType.Hospital), AllFloorsProvider.Instance.HospitalFloorsProvider);

            ViewModelLocationProvider.SetDefaultViewModelFactory(instancesCache.GetOrCreateInstance);
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(viewMappingCache.GetViewModelType);

            this.Subscribe<MessageShowFlatIndications>(OnShowFlatIndicationsRequest);
            this.Subscribe<MessageShowFloorIndications>(OnShowFloorIndicationsRequest);
            this.Subscribe<MessageShowNotes<SubjectEntityViewModel>>(OnShowNotesRequest);
            this.Subscribe<MessageShowPhotos<SubjectEntityViewModel>>(OnShowFloorPhotosRequest);
        }


        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) 
        {
            string message = $"Произошла ошибка.{Environment.NewLine}{e.Exception.GetRootException().Message}";

            logger.Error(message);

            MessageBox.Show(message);

            Environment.Exit(-6);
        }


        private void SimpleFlatView_OnEventShowFlatDetails(FlatDecoratorViewModel flat) 
        {
            this.Publish(new MessageShowFlatDetails(flat));
        }

        private void SimpleFlatView_OnEventShowOwnerDetails(FlatDecoratorViewModel flat) 
        {
            this.Publish(new MessageShowAborigenDetails(flat.OwnerDecorator));
        }


        private void OnShowFlatIndicationsRequest(MessageShowFlatIndications message) 
        {
            var win = CreateHostWindow($"{ LocRes.Indications } квартиры { message.FlatDecorator.Flat.Number }");
            win.Show();
        }

        private void OnShowFloorIndicationsRequest(MessageShowFloorIndications message) 
        {
            var win = CreateHostWindow(
                $"{ LocRes.Indications } { message.FloorDecorator.OriginaFloorViewModel.FloorNumber} этажа " +
                $"секции '{ message.FloorDecorator.OriginaFloorViewModel.Section }'");

            win.Show();
        }

        private void OnShowNotesRequest(MessageShowNotes<SubjectEntityViewModel> message) 
        {
            Window window = CreateHostWindow(message.AttachablesOwner.SubjectFriendlyInfo);

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Star) });
            var notesView = new SubjectNotesView { Margin = (Thickness)Resources["4TopMargin"] };
            var toolBar = new AttachmentsToolbarView();
            Grid.SetRow(toolBar, 1);
            Grid.SetRow(notesView, 2);
            grid.Children.Add(toolBar);
            grid.Children.Add(notesView);
            window.Content = grid;

            var context = (SubjectNotesViewModel)notesView.DataContext;
            context.SetNotesOwner(message.AttachablesOwner);

            window.Show();
        }

        private void OnShowFloorPhotosRequest(MessageShowPhotos<SubjectEntityViewModel> message) 
        {
            Window window = CreateHostWindow(message.AttachablesOwner.SubjectFriendlyInfo);

            window.Content = "страница фотографий";

            window.Show();
        }


        private Window CreateHostWindow(string title) 
        {
            Window window = Extensions.WindowExtensions.CreateEmptyHorizontalWindow();
            window.Title = title;

            window.MakeSticky();

            return window;
        }
    }
}