using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TwinSovet.Helpers;
using TwinSovet.ViewModels;
using TwinSovet.ViewModels.Subjects;


namespace TwinSovet.Views 
{
    /// <summary>
    /// Логика взаимодействия для FlatCardView.xaml
    /// </summary>
    public partial class FlatCardView : UserControl 
    {
        /// <summary>
        /// Событие запроса на редактирование инфы о владельце квартиры.
        /// </summary>
        internal event Action<FlatDecoratorViewModel> EventRequestEditOwner = flatModel => { };
        
        
        public FlatCardView() 
        {
            InitializeComponent();
        }


        public void FocusInnerBox() 
        {
            Keyboard.Focus(NumberBox);
        }


        private void OwnerButton_OnClick(object sender, RoutedEventArgs e) 
        {
            var flatDecorator = (FlatDecoratorViewModel)DataContext;
            EventRequestEditOwner(flatDecorator);
        }
    }
}