using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

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
        private Storyboard inAnimation;
        private Storyboard outAnimation;


        public MainView() 
        {
            InitializeComponent();

            inAnimation = (Storyboard)Resources["DetailedFlat_In_Animation"];
            outAnimation = (Storyboard)Resources["DetailedFlat_Out_Animation"];
        }


        public static readonly DependencyProperty DetailedFlatViewModelProperty = 
            DependencyProperty.Register(nameof(DetailedFlatViewModel), typeof(FlatViewModel), typeof(MainView));

        public static readonly DependencyProperty IsShowingFlatDetailsProperty = 
            DependencyProperty.Register(nameof(IsShowingFlatDetails), typeof(bool), typeof(MainView), new FrameworkPropertyMetadata(OnIsShowingDetailsChanged));

        
        public bool IsShowingFlatDetails 
        {
            get => (bool)GetValue(IsShowingFlatDetailsProperty);
            set => SetValue(IsShowingFlatDetailsProperty, value);
        }

        internal FlatViewModel DetailedFlatViewModel 
        {
            get => (FlatViewModel)GetValue(DetailedFlatViewModelProperty);
            set => SetValue(DetailedFlatViewModelProperty, value);
        }


        private static void OnIsShowingDetailsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) 
        {
            var view = (MainView)sender;

            if ((bool)e.NewValue)
            {
                view.inAnimation.Begin();
            }
            else
            {
                view.outAnimation.Begin();
            }
        }


        private void FirstSectionPlanView_OnEventShowFlatDetails(FlatViewModel flatModel) 
        {
            DetailedFlatViewModel = flatModel;
            IsShowingFlatDetails = true;
        }

        private void EscapeCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e) 
        {
            if (IsShowingFlatDetails)
            {
                IsShowingFlatDetails = false;
            }
        }
    }
}