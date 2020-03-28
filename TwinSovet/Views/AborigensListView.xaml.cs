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
using Microsoft.Practices.Unity;
using TwinSovet.Attributes;
using TwinSovet.Helpers;
using TwinSovet.ViewModels;
using TwinSovet.ViewModels.Subjects;


namespace TwinSovet.Views 
{
    /// <summary>
    /// Interaction logic for AborigensListView.xaml
    /// </summary>
    [HasViewModel(typeof(AborigensListViewModel))]
    public partial class AborigensListView : UserControl 
    {
        internal event Action<AborigenDecoratorViewModel> EventShowAborigenDetais = aborigen => { };


        public AborigensListView() 
        {
            InitializeComponent();

            IsVisibleChanged += OnIsVisibleChanged;
        }


        private AborigensListViewModel ViewModel { get; set; }


        public void FocusSearchBox() 
        {
            AborigensFilterer.FocusInnerBox();
        }


        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) 
        {
            if ((bool)e.NewValue == false || DataContext is AborigensListViewModel) return;

            NonDesignInvoker.Invoke(this, () =>
            {
                DataContext = MainContainer.Instance.Resolve<AborigensListViewModel>();
                ViewModel = (AborigensListViewModel)DataContext;
            });
        }

        private void SimpleAborigenView_OnShowAborigenDetais(AborigenDecoratorViewModel aborigen) 
        {
            EventShowAborigenDetais(aborigen);
        }
    }
}