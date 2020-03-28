using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Practices.Unity;
using TwinSovet.Attributes;
using TwinSovet.Data.Enums;
using TwinSovet.Helpers;
using TwinSovet.Providers;
using TwinSovet.ViewModels;
using TwinSovet.ViewModels.Subjects;


namespace TwinSovet.Views 
{
    /// <summary>
    /// Логика взаимодействия для SectionPlanView.xaml
    /// </summary>
    public partial class SectionPlanView : UserControl 
    {
        private readonly DelayedEventInvoker delayedFocuser = new DelayedEventInvoker(StaticsProvider.SearchDelay);

        internal event Action<FlatDecoratorViewModel> EventShowFlatDetails = flatDecorator => { };
        internal event Action<AborigenDecoratorViewModel> EventShowAborigenDetails = aborigenDecorator => { };


        public SectionPlanView() 
        {
            InitializeComponent();

            Progresser.Maximum = StaticsProvider.FlatsInFurnitureSection;

            delayedFocuser.DelayedEvent += DelayedFocuser_OnDelayedEvent;

            NonDesignInvoker.Invoke(this, () =>
            {
                var binding = new Binding
                {
                    Mode = BindingMode.OneWay,
                    Path = new PropertyPath(nameof(SectionViewModelBase.FloorWrappersCollection))
                };

                FloorsListBox.SetBinding(ItemsControl.ItemsSourceProperty, binding);
            });

            IsVisibleChanged += OnIsVisibleChanged;
        }


        public static readonly DependencyProperty TypeOFSectionProperty = 
            DependencyProperty.Register(nameof(TypeOFSection), typeof(SectionType), 
                typeof(SectionPlanView), new FrameworkPropertyMetadata(TypeOFSection_OnChanged));


        public SectionType TypeOFSection
        {
            get => (SectionType) GetValue(TypeOFSectionProperty);
            set => SetValue(TypeOFSectionProperty, value);
        }


        private static void TypeOFSection_OnChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) 
        {
            var view = (SectionPlanView) sender;
            var sectionType = (SectionType)e.NewValue;

            if (view.IsVisible)
            {
                view.SetContext();
            }
        }


        private void DelayedFocuser_OnDelayedEvent() 
        {
            FloorsFilter.FocusInnerBox();
        }

        private void FloorsFilter_OnLoaded(object sender, RoutedEventArgs e) 
        {
            delayedFocuser.RequestDelayedEvent();
        }

        private void FloorView_OnEventShowFlatDetails(FlatDecoratorViewModel flat) 
        {
            EventShowFlatDetails(flat);
        }

        private void FloorView_OnEventShowAborigenDetails(AborigenDecoratorViewModel decorator) 
        {
            EventShowAborigenDetails(decorator);
        }

        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) 
        {
            if ((bool)e.NewValue == false || DataContext is SectionViewModelBase) return;

            SetContext();
        }


        private void SetContext() 
        {
            if (TypeOFSection == SectionType.Furniture)
            {
                DataContext = MainContainer.Instance.Resolve<FurnitureSectionPlanViewModel>();
            }
            else if (TypeOFSection == SectionType.Hospital)
            {
                DataContext = MainContainer.Instance.Resolve<HospitalSectionPlanViewModel>();
            }
        }
    }
}