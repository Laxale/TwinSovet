using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using TwinSovet.ViewModels;
using TwinSovet.ViewModels.Subjects;


namespace TwinSovet.Views 
{
    /// <summary>
    /// Простая панелька отображения квартиры в списках.
    /// </summary>
    public partial class SimpleFlatView : UserControl 
    {
        internal event Action<FlatDecoratorViewModel> EventShowFlatDetails = flatDecorator => { };
        internal event Action<FlatDecoratorViewModel> EventShowOwnerDetails = flatDecorator => { };


        public SimpleFlatView() 
        {
            InitializeComponent();
        }


        private void SimpleFlatView_OnMouseDown(object sender, MouseButtonEventArgs e) 
        {
            var flatDecorator = (FlatDecoratorViewModel)DataContext;
            EventShowFlatDetails(flatDecorator);
        }

        private void EditOwnerButton_OnClick(object sender, RoutedEventArgs e) 
        {
            var flatDecorator = (FlatDecoratorViewModel)DataContext;
            EventShowOwnerDetails(flatDecorator);
        }
    }
}