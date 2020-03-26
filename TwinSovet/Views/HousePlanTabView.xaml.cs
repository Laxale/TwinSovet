using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Prism.Regions;
using PubSub;
using TwinSovet.Messages.Details;
using TwinSovet.Providers;
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
        private readonly Storyboard innerEditOwner_In_Animation;
        private readonly Storyboard innerEditOwner_Out_Animation;
        private readonly Storyboard detailedAborigen_In_Animation;
        private readonly Storyboard detailedAborigen_Out_Animation;
        

        public HousePlanTabView() 
        {
            InitializeComponent();

            inFlatAnimation = (Storyboard)Resources["DetailedFlat_In_Animation"];
            outFlatAnimation = (Storyboard)Resources["DetailedFlat_Out_Animation"];
            innerEditOwner_In_Animation = (Storyboard)Resources["CreateOwner_In_Animation"];
            innerEditOwner_Out_Animation = (Storyboard)Resources["CreateOwner_Out_Animation"];
            detailedAborigen_In_Animation = (Storyboard)Resources["DetailedAborigen_In_Animation"];
            detailedAborigen_Out_Animation = (Storyboard)Resources["DetailedAborigen_Out_Animation"];

            inFlatAnimation.Completed += InFlatAnimation_OnCompleted;
            outFlatAnimation.Completed += OutFlatAnimation_OnCompleted;
            innerEditOwner_Out_Animation.Completed += InnerEditOwnerOutAnimation_OnCompleted;
            detailedAborigen_In_Animation.Completed += DetailedAborigenInAnimation_OnCompleted;
            detailedAborigen_Out_Animation.Completed += DetailedAborigenOutAnimation_OnCompleted;

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e) 
        {
            this.Subscribe<MessageShowFlatDetails>(OnShowFlatDetailsRequest);
            this.Subscribe<MessageShowAborigenDetails>(OnShowAborigenDetails);
        }


        public static readonly DependencyProperty OwnerPanelHeightProperty =
            DependencyProperty.Register(nameof(OwnerPanelHeight), typeof(double), typeof(HousePlanTabView));

        public static readonly DependencyProperty DetailedFlatDecoratorProperty =
            DependencyProperty.Register(nameof(DetailedFlatDecorator), typeof(FlatDecoratorViewModel), 
                typeof(HousePlanTabView), new FrameworkPropertyMetadata(DetailedFlatDecorator_OnChanged));

        public static readonly DependencyProperty DetailedAborigenDecoratorProperty = 
            DependencyProperty.Register(nameof(DetailedAborigenDecorator), typeof(AborigenDecoratorViewModel), 
                typeof(HousePlanTabView), new FrameworkPropertyMetadata(DetailedAborigen_OnChanged));

        
        internal AborigenDecoratorViewModel DetailedAborigenDecorator 
        {
            get => (AborigenDecoratorViewModel) GetValue(DetailedAborigenDecoratorProperty);
            set => SetValue(DetailedAborigenDecoratorProperty, value);
        }

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


        private static void DetailedFlatDecorator_OnChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) 
        {
            var planView = (HousePlanTabView)sender;

            void AborigenEditable_OnExecutedSaveAborigen()
            {
                planView.innerEditOwner_Out_Animation.Begin();
                FlatsProvider.CommandSave.RaiseCanExecuteChanged();
            }

            void FlatDecorator_OnFlatSaved(FlatDecoratorViewModel decorator)
            {
                planView.outFlatAnimation.Begin();
            }

            if (e.OldValue is FlatDecoratorViewModel oldDecorator)
            {
                oldDecorator.EventFlatSaved -= FlatDecorator_OnFlatSaved;
                oldDecorator.OwnerDecorator.AborigenEditable.EventExecutedSaveAborigen -= AborigenEditable_OnExecutedSaveAborigen;
            }

            var flatDecorator = (FlatDecoratorViewModel) e.NewValue;

            flatDecorator.EventFlatSaved += FlatDecorator_OnFlatSaved;
            flatDecorator.OwnerDecorator.AborigenEditable.EventExecutedSaveAborigen += 
                AborigenEditable_OnExecutedSaveAborigen;
        }

        private static void DetailedAborigen_OnChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) 
        {
            var planView = (HousePlanTabView)sender;
            if (e.OldValue is AborigenDecoratorViewModel oldDecor)
            {
                oldDecor.AborigenEditable.EventExecutedSaveAborigen -= planView.AborigenEditable_OnExecutedSaveAborigen;
            }

            if (e.NewValue is AborigenDecoratorViewModel newDecor)
            {
                newDecor.AborigenEditable.EventExecutedSaveAborigen += planView.AborigenEditable_OnExecutedSaveAborigen;
            }
        }


        private void HomePlan_EscapeCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e) 
        {
            if (CreateOwnerPanel.ActualHeight > 10 && CreateOwnerPanel.Visibility == Visibility.Visible)
            {
                innerEditOwner_Out_Animation.Begin();
            }
            else if (DetailedFlatPanel.Width > 10)
            {
                outFlatAnimation.Begin();
            }
            else
            {
                DetailedAborigenDecorator = null;
                detailedAborigen_Out_Animation.Begin();
            }
        }


        private void InFlatAnimation_OnCompleted(object sender, EventArgs e) 
        {
            DetailedFlatCard.FocusInnerBox();
        }

        private void OutFlatAnimation_OnCompleted(object sender, EventArgs e) 
        {
            HideMask();
        }

        private void InnerEditOwnerOutAnimation_OnCompleted(object sender, EventArgs eventArgs) 
        {
            DetailedFlatCard.FocusInnerBox();
            CreateOwnerPanel.Visibility = Visibility.Hidden;
        }

        private void DetailedAborigenInAnimation_OnCompleted(object sender, EventArgs e) 
        {
            DetailedAborigenPanel.FocusInnerBox();
        }

        private void DetailedAborigenOutAnimation_OnCompleted(object sender, EventArgs e) 
        {
            HideMask();
            DetailedAborigenDecorator = null;
        }

        private void DetailedFlatCard_OnEventRequestEditOwner(FlatDecoratorViewModel flatDecorator) 
        {
            if (CreateOwnerPanel.Visibility == Visibility.Visible)
            {
                innerEditOwner_Out_Animation.Begin();
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
            innerEditOwner_Out_Animation.Begin();
        }

        private void CancelFlatDetailsButton_OnClick(object sender, RoutedEventArgs e) 
        {
            outFlatAnimation.Begin();
        }

        private void SectionsMaskPanel_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) 
        {
            if (DetailedFlatPanel.Width > 10)
            {
                outFlatAnimation.Begin();
            }
            else 
            {
                DetailedAborigenDecorator = null;
                detailedAborigen_Out_Animation.Begin();
            }
        }

        private void DetailedAborigenPanel_OnEventCancellationRequest() 
        {
            detailedAborigen_Out_Animation.Begin();
        }

        private void FirstSectionView_OnEventShowFlatDetails(FlatDecoratorViewModel flat) 
        {
            ShowFlatDetails(flat);
        }

        private void FirstSectionView_OnEventShowAborigenDetails(AborigenDecoratorViewModel aborigen) 
        {
            ShowAborigenDetails(aborigen);
        }

        private void AborigenEditable_OnExecutedSaveAborigen() 
        {
            DetailedAborigenDecorator = null;
            detailedAborigen_Out_Animation.Begin();
        }


        private void OnShowFlatDetailsRequest(MessageShowFlatDetails message) 
        {
            ShowFlatDetails(message.ViewModel);
        }

        private void OnShowAborigenDetails(MessageShowAborigenDetails message) 
        {
            ShowAborigenDetails(message.ViewModel);
        }


        private void ShowMask() 
        {
            HomePlanMaskPanel.Visibility = Visibility.Visible;
        }

        private void HideMask() 
        {
            HomePlanMaskPanel.Visibility = Visibility.Collapsed;
        }

        private void AnimateOwnerCreationIn() 
        {
            CreateOwnerPanel.Visibility = Visibility.Visible;
            innerEditOwner_In_Animation.Begin();
        }

        private void ShowFlatDetails(FlatDecoratorViewModel flat) 
        {
            ShowMask();
            DetailedFlatDecorator = flat;
            inFlatAnimation.Begin();
        }

        private void ShowAborigenDetails(AborigenDecoratorViewModel aborigen) 
        {
            ShowMask();
            DetailedAborigenDecorator = aborigen;
            detailedAborigen_In_Animation.Begin();
        }
    }
}