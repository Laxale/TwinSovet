using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

using TwinSovet.Data.Models;
using TwinSovet.ViewModels;


namespace TwinSovet.Views 
{
    /// <summary>
    /// Логика взаимодействия для HousePlanTabView.xaml
    /// </summary>
    public partial class HousePlanTabView : UserControl 
    {
        private readonly Storyboard inFlatAnimation;
        private readonly Storyboard outFlatAnimation;
        private readonly Storyboard createOwner_In_Animation;
        private readonly Storyboard createOwner_Out_Animation;
        

        public HousePlanTabView() 
        {
            InitializeComponent();

            inFlatAnimation = (Storyboard)Resources["DetailedFlat_In_Animation"];
            outFlatAnimation = (Storyboard)Resources["DetailedFlat_Out_Animation"];
            createOwner_In_Animation = (Storyboard)Resources["CreateOwner_In_Animation"];
            createOwner_Out_Animation = (Storyboard)Resources["CreateOwner_Out_Animation"];

            inFlatAnimation.Completed += InFlatAnimation_OnCompleted;
            createOwner_Out_Animation.Completed += CreateOwnerOutAnimation_OnCompleted;
        }


        public static readonly DependencyProperty OwnerPanelHeightProperty =
            DependencyProperty.Register(nameof(OwnerPanelHeight), typeof(double), typeof(HousePlanTabView));

        public static readonly DependencyProperty DetailedFlatDecoratorProperty =
            DependencyProperty.Register(nameof(DetailedFlatDecorator), typeof(FlatDecoratorViewModel), typeof(HousePlanTabView));

        public static readonly DependencyProperty IsShowingFlatDetailsProperty =
            DependencyProperty.Register(nameof(IsShowingFlatDetails), typeof(bool), typeof(HousePlanTabView), new FrameworkPropertyMetadata(OnIsShowingDetailsChanged));


        public double OwnerPanelHeight 
        {
            get => (double)GetValue(OwnerPanelHeightProperty);
            private set => SetValue(OwnerPanelHeightProperty, value);
        }

        /// <summary>
        /// Возвращает или задаёт вьюмодель выбранной для детализации квартиры.
        /// </summary>
        internal FlatDecoratorViewModel DetailedFlatDecorator 
        {
            get => (FlatDecoratorViewModel)GetValue(DetailedFlatDecoratorProperty);
            set => SetValue(DetailedFlatDecoratorProperty, value);
        }

        /// <summary>
        /// Возвращает или задаёт флаг - отображаются ли сейчас детали той или иной выбранной квартиры.
        /// </summary>
        public bool IsShowingFlatDetails 
        {
            get => (bool)GetValue(IsShowingFlatDetailsProperty);
            set => SetValue(IsShowingFlatDetailsProperty, value);
        }


        private static void OnIsShowingDetailsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) 
        {
            var view = (HousePlanTabView)sender;

            if ((bool)e.NewValue)
            {
                view.inFlatAnimation.Begin();
                view.HomePlanMaskPanel.Visibility = Visibility.Visible;
            }
            else
            {
                view.outFlatAnimation.Begin();
                view.HomePlanMaskPanel.Visibility = Visibility.Collapsed;
            }
        }


        private void HomePlan_EscapeCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e) 
        {
            if (CreateOwnerPanel.ActualHeight > 10 && CreateOwnerPanel.Visibility == Visibility.Visible)
            {
                createOwner_Out_Animation.Begin();
            }
            else if (IsShowingFlatDetails)
            {
                IsShowingFlatDetails = false;
            }
        }


        private void InFlatAnimation_OnCompleted(object sender, EventArgs e) 
        {
            DetailedFlatCard.FocusInnerBox();
        }

        private void CreateOwnerOutAnimation_OnCompleted(object sender, EventArgs eventArgs) 
        {
            CreateOwnerPanel.Visibility = Visibility.Hidden;
        }

        private void FirstSectionPlanView_OnEventShowFlatDetails(FlatDecoratorViewModel flatDecorator) 
        {
            IsShowingFlatDetails = true;
            DetailedFlatDecorator = flatDecorator;
        }

        private void FlatCardView_OnEventRequestOwnerCreation(FlatViewModel flatModel) 
        {
            if (CreateOwnerPanel.Visibility == Visibility.Visible)
            {
                createOwner_Out_Animation.Begin();
            }
            else
            {
                AnimateOwnerCreationIn();
            }
        }

        private void OwnerPanel_OnLoaded(object sender, RoutedEventArgs e) 
        {
            OwnerPanelHeight = CreateOwnerPanel.ActualHeight;
        }

        private void CancelOwnerCreationButton_OnClick(object sender, RoutedEventArgs e) 
        {
            createOwner_Out_Animation.Begin();
        }

        private void CancelFlatDetailsButton_OnClick(object sender, RoutedEventArgs e) 
        {
            IsShowingFlatDetails = false;
        }

        private void SectionsMaskPanel_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) 
        {
            IsShowingFlatDetails = false;
        }


        private void AnimateOwnerCreationIn() 
        {
            if (!DetailedFlatDecorator.HasOwner)
            {
                DetailedFlatDecorator.Owner = new AborigenDecoratorViewModel(AborigenViewModel.CreateEditable(new AborigenModel()));
            }
            CreateOwnerPanel.Visibility = Visibility.Visible;
            createOwner_In_Animation.Begin();
        }
    }
}