using System;
using System.Windows;
using System.Windows.Controls;

using TwinSovet.Attributes;
using TwinSovet.Helpers;
using TwinSovet.Providers;
using TwinSovet.ViewModels;


namespace TwinSovet.Views 
{
    /// <summary>
    /// Логика взаимодействия для FirstSectionPlanView.xaml
    /// </summary>
    [HasViewModel(typeof(FirstSectionPlanViewModel))]
    public partial class FirstSectionPlanView : UserControl 
    {
        private readonly DelayedEventInvoker delayedFocuser = new DelayedEventInvoker(StaticsProvider.SearchDelay);

        internal event Action<FlatDecoratorViewModel> EventShowFlatDetails = flatDecorator => { };
        internal event Action<AborigenDecoratorViewModel> EventShowAborigenDetails = aborigenDecorator => { };


        public FirstSectionPlanView() 
        {
            InitializeComponent();

            delayedFocuser.DelayedEvent += DelayedFocuser_OnDelayedEvent;
        }


        private void DelayedFocuser_OnDelayedEvent() 
        {
            FloorsFilter.FocusInnerBox();
        }

        private void FloorsFilter_OnLoaded(object sender, RoutedEventArgs e) 
        {
            delayedFocuser.RequestDelayedEvent();
        }

        private void FloorView_OnEventShowFlatDetails(FlatDecoratorViewModel flat) 
        {
            EventShowFlatDetails(flat);
        }

        private void FloorView_OnEventShowAborigenDetails(AborigenDecoratorViewModel decorator) 
        {
            EventShowAborigenDetails(decorator);
        }
    }
}