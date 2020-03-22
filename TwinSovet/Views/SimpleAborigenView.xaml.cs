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

using TwinSovet.Data.Models;
using TwinSovet.ViewModels;


namespace TwinSovet.Views
{
    /// <summary>
    /// Interaction logic for SimpleAborigenView.xaml
    /// </summary>
    public partial class SimpleAborigenView : UserControl 
    {
        internal event Action<FlatAborigenViewModel> EventShowAborigenDetais = aborigen => { };


        public SimpleAborigenView() 
        {
            InitializeComponent();
        }


        private void AborigenButton_OnClick(object sender, RoutedEventArgs e) 
        {
            EventShowAborigenDetais(((AborigenInListDecoratorViewModel)DataContext).Aborigen);
        }
    }
}