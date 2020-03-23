using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using TwinSovet.Attributes;
using TwinSovet.ViewModels;


namespace TwinSovet.Views 
{
    /// <summary>
    /// Логика взаимодействия для MainView.xaml
    /// </summary>
    [HasViewModel(typeof(MainViewModel))]
    public partial class MainView : UserControl 
    {
        public MainView() 
        {
            InitializeComponent();
        }
    }
}