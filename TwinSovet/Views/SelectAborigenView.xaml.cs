using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Common.Helpers;
using NLog.Filters;
using TwinSovet.Attributes;
using TwinSovet.Controls;
using TwinSovet.Helpers;
using TwinSovet.Providers;
using TwinSovet.ViewModels;


namespace TwinSovet.Views 
{
    /// <summary>
    /// Логика взаимодействия для SelectAborigenView.xaml
    /// </summary>
    [HasViewModel(typeof(SelectAborigenViewModel))]
    public partial class SelectAborigenView : UserControl 
    {
        private readonly DelayedEventInvoker delayedFocuser = new DelayedEventInvoker(StaticsProvider.SearchDelay);

        internal event Action<AborigenDecoratorViewModel> EventAborigenSelected = decorator => { };


        public SelectAborigenView() 
        {
            InitializeComponent();

            delayedFocuser.DelayedEvent += DelayedFocuser_OnDelayedEvent;
        }

        ~SelectAborigenView() 
        {
            delayedFocuser.Dispose();
        }


        private void DelayedFocuser_OnDelayedEvent() 
        {
            DispatcherHelper.BeginInvokeOnDispatcher(() => Focuser.FocusInnerBox());
        }

        private void ListBoxItem_OnDoubleClick(object sender, MouseButtonEventArgs e) 
        {
            var decorator = (AborigenDecoratorViewModel)((ListBoxItem) sender).DataContext;
            EventAborigenSelected(decorator);
        }

        private void Filterer_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) 
        {
            delayedFocuser.RequestDelayedEvent();
        }


        private void AcceptCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e) 
        {
            var decorator =
                AborigensList.SelectedItem is AborigenDecoratorViewModel aborigenDecorator ?
                    aborigenDecorator : 
                    null;

            EventAborigenSelected(decorator);
        }
    }
}