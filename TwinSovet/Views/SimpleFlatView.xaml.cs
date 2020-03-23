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

using TwinSovet.ViewModels;


namespace TwinSovet.Views
{
    /// <summary>
    /// Логика взаимодействия для SimpleFlatView.xaml
    /// </summary>
    public partial class SimpleFlatView : UserControl 
    {
        internal event Action<FlatDecoratorViewModel> EventShowFlatDetails = flatModel => { };


        public SimpleFlatView() 
        {
            InitializeComponent();
        }


        private void SimpleFlatView_OnMouseDown(object sender, MouseButtonEventArgs e) 
        {
            var flatDecorator = (FlatDecoratorViewModel)DataContext;
            EventShowFlatDetails(flatDecorator);
        }
    }
}