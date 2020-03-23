using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TwinSovet.Helpers;
using TwinSovet.ViewModels;


namespace TwinSovet.Views 
{
    /// <summary>
    /// Логика взаимодействия для FlatCardView.xaml
    /// </summary>
    public partial class FlatCardView : UserControl 
    {
        /// <summary>
        /// Событие запроса на показ инфы о владельце квартиры.
        /// </summary>
        internal event Action<FlatViewModel> EventRequestShowOwner = flatModel => { };
        
        /// <summary>
        /// Событие запроса на создание владельца квартиры.
        /// </summary>
        internal event Action<FlatViewModel> EventRequestOwnerCreation = flatModel => { };


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
            var flatDecorator = (FlatInListDecoratorViewModel)DataContext;
            if (flatDecorator.HasOwner)
            {
                EventRequestShowOwner(flatDecorator.Flat);
            }
            else
            {
                EventRequestOwnerCreation(flatDecorator.Flat);
            }
        }
    }
}