using System;
using System.Collections.Generic;
using System.ComponentModel;
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

using PubSub;

using TwinSovet.Messages;
using TwinSovet.ViewModels;


namespace TwinSovet.Views 
{
    /// <summary>
    /// Логика взаимодействия для FloorView.xaml
    /// </summary>
    public partial class FloorView : UserControl 
    {
        internal event Action<FlatViewModel> EventShowFlatDetails = flatModel => { };


        public FloorView()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ViewModel = (FloorViewModel)DataContext;
            this.Subscribe<MessageShowFlatDetails>(OnShowFlatDetailsRequest);
        }


        public static readonly DependencyProperty RoomsViewProperty = 
            DependencyProperty.Register(nameof(RoomsView), typeof(ICollectionView), typeof(FloorView));


        public ICollectionView RoomsView 
        {
            get => (ICollectionView) GetValue(RoomsViewProperty);
            set => SetValue(RoomsViewProperty, value);
        }


        private FloorViewModel ViewModel { get; set; }


        private void OnShowFlatDetailsRequest(MessageShowFlatDetails message) 
        {
            if (ViewModel.FlatsView.SourceCollection.OfType<FlatViewModel>().Contains(message.Flat))
            {
                EventShowFlatDetails(message.Flat);
            }
        }
        
        private void SimpleFlatView_OnEventShowFlatDetails(FlatViewModel flatModel) 
        {
            EventShowFlatDetails(flatModel);
        }
    }
}