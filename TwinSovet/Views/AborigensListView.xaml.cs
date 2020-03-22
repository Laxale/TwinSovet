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

using TwinSovet.Attributes;
using TwinSovet.ViewModels;


namespace TwinSovet.Views 
{
    /// <summary>
    /// Interaction logic for AborigensListView.xaml
    /// </summary>
    [HasViewModel(typeof(AborigensListViewModel))]
    public partial class AborigensListView : UserControl 
    {
        internal event Action<FlatAborigenViewModel> EventShowAborigenDetais = aborigen => { };


        public AborigensListView()
        {
            InitializeComponent();
        }


        private void SimpleAborigenView_OnShowAborigenDetais(FlatAborigenViewModel aborigen) 
        {
            EventShowAborigenDetais(aborigen);
        }
    }
}