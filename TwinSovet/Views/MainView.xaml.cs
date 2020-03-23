using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

using PubSub;

using StickyWindows;
using StickyWindows.WPF;

using TwinSovet.Attributes;
using TwinSovet.Extensions;
using TwinSovet.Messages;
using TwinSovet.ViewModels;
using WindowExtensions = StickyWindows.WPF.WindowExtensions;


namespace TwinSovet.Views 
{
    /// <summary>
    /// Логика взаимодействия для MainView.xaml
    /// </summary>
    [HasViewModel(typeof(MainViewModel))]
    public partial class MainView : UserControl 
    {
        private Storyboard inFlatAnimation;
        private Storyboard outFlatAnimation;
        private Storyboard createOwner_In_Animation;
        private Storyboard createOwner_Out_Animation;
        private Storyboard detailedAborigen_In_Animation;
        private Storyboard detailedAborigen_Out_Animation;
        


        public MainView() 
        {
            InitializeComponent();

            inFlatAnimation = (Storyboard)Resources["DetailedFlat_In_Animation"];
            outFlatAnimation = (Storyboard)Resources["DetailedFlat_Out_Animation"];
            createOwner_In_Animation = (Storyboard)Resources["CreateOwner_In_Animation"];
            createOwner_Out_Animation = (Storyboard)Resources["CreateOwner_Out_Animation"];
            detailedAborigen_In_Animation = (Storyboard)Resources["DetailedAborigen_In_Animation"];
            detailedAborigen_Out_Animation = (Storyboard)Resources["DetailedAborigen_Out_Animation"];

            inFlatAnimation.Completed += InFlatAnimation_OnCompleted;
            createOwner_Out_Animation.Completed += CreateOwnerOutAnimation_OnCompleted;

            this.Subscribe<MessageShowNotes<FloorViewModel>>(OnShowNotesRequest);
            this.Subscribe<MessageShowPhotos<FloorViewModel>>(OnShowPhotosRequest);
        }


        public static readonly DependencyProperty DetailedFlatViewModelProperty = 
            DependencyProperty.Register(nameof(DetailedFlatViewModel), typeof(FlatInListDecoratorViewModel), typeof(MainView));

        public static readonly DependencyProperty IsShowingFlatDetailsProperty = 
            DependencyProperty.Register(nameof(IsShowingFlatDetails), typeof(bool), typeof(MainView), new FrameworkPropertyMetadata(OnIsShowingDetailsChanged));

        public static readonly DependencyProperty OwnerPanelHeightProperty = 
            DependencyProperty.Register(nameof(OwnerPanelHeight), typeof(double), typeof(MainView));

        public static readonly DependencyProperty DetailedAborigenViewModelProperty = 
            DependencyProperty.Register(nameof(DetailedAborigenViewModel), typeof(AborigenViewModel), typeof(MainView));


        /// <summary>
        /// Возвращает или задаёт вьюмодель выбранноГо для детализации жителя.
        /// </summary>
        internal AborigenViewModel DetailedAborigenViewModel 
        {
            get => (AborigenViewModel)GetValue(DetailedAborigenViewModelProperty);
            set => SetValue(DetailedAborigenViewModelProperty, value);
        }

        public double OwnerPanelHeight 
        {
            get => (double)GetValue(OwnerPanelHeightProperty);
            private set => SetValue(OwnerPanelHeightProperty, value);
        }
        
        /// <summary>
        /// Возвращает или задаёт флаг - отображаются ли сейчас детали той или иной выбранной квартиры.
        /// </summary>
        public bool IsShowingFlatDetails 
        {
            get => (bool)GetValue(IsShowingFlatDetailsProperty);
            set => SetValue(IsShowingFlatDetailsProperty, value);
        }

        /// <summary>
        /// Возвращает или задаёт вьюмодель выбранной для детализации квартиры.
        /// </summary>
        internal FlatInListDecoratorViewModel DetailedFlatViewModel 
        {
            get => (FlatInListDecoratorViewModel)GetValue(DetailedFlatViewModelProperty);
            set => SetValue(DetailedFlatViewModelProperty, value);
        }


        private static void OnIsShowingDetailsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) 
        {
            var view = (MainView)sender;

            if ((bool)e.NewValue)
            {
                view.inFlatAnimation.Begin();
                view.MaskPanel.Visibility = Visibility.Visible;
            }
            else
            {
                view.outFlatAnimation.Begin();
                view.MaskPanel.Visibility = Visibility.Collapsed;
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
        
        private void FirstSectionPlanView_OnEventShowFlatDetails(FlatInListDecoratorViewModel flatModel) 
        {
            DetailedFlatViewModel = flatModel;
            IsShowingFlatDetails = true;
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

        private void AnimateOwnerCreationIn()
        {
            CreateOwnerPanel.Visibility = Visibility.Visible;
            createOwner_In_Animation.Begin();
        }

        private void OwnerPanel_OnLoaded(object sender, RoutedEventArgs e) 
        {
            OwnerPanelHeight = CreateOwnerPanel.ActualHeight;
        }

        private void CancelOwnerCreationButton_OnClick(object sender, RoutedEventArgs e) 
        {
            createOwner_Out_Animation.Begin();
        }

        private void AborigensListView_OnEventShowAborigenDetais(AborigenViewModel aborigen) 
        {
            DetailedAborigenViewModel = aborigen;
            detailedAborigen_In_Animation.Begin();
            DetailedAborigenCard.FocusInnerBox();
        }

        private void AborigensPage_EscapeCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e) 
        {
            AnimateAborigenDetailsOut();
            AborigensFilterer.FocusInnerBox();
        }

        private void CancelAborigenDetailsButton_OnClick(object sender, RoutedEventArgs e) 
        {
            AnimateAborigenDetailsOut();
            AborigensFilterer.FocusInnerBox();
        }


        private void OnShowNotesRequest(MessageShowNotes<FloorViewModel> message) 
        {
            Window window = CreateHostWindow();

            window.Content = new NotesView();

            window.Show();
        }

        private void OnShowPhotosRequest(MessageShowPhotos<FloorViewModel> message) 
        {
            Window window = CreateHostWindow();

            window.Content = "страница фотографий";

            window.Show();
        }


        private Window CreateHostWindow() 
        {
            Window window = Extensions.WindowExtensions.CreateEmptyHorizontalWindow();

            window.MakeSticky();

            return window;
        }
        
        private void AnimateAborigenDetailsOut() 
        {
            if (DetailedAborigenPanel.Width > 10)
            {
                detailedAborigen_Out_Animation.Begin();
            }
        }

        private void CancelFlatDetailsButton_OnClick(object sender, RoutedEventArgs e) 
        {
            IsShowingFlatDetails = false;
        }

        private void SectionsMaskPanel_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) 
        {
            IsShowingFlatDetails = false;
        }
    }
}