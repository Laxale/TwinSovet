using System;
using System.Windows;
using System.Windows.Controls;

using TwinSovet.ViewModels;


namespace TwinSovet.Views
{
    /// <summary>
    /// Логика взаимодействия для FlatCardView.xaml
    /// </summary>
    public partial class FlatCardView : UserControl 
    {
        internal event Action<FlatViewModel> EventRequestOwnerCreation = flatModel => { };


        public FlatCardView()
        {
            InitializeComponent();
        }


        private void OwnerButton_OnClick(object sender, RoutedEventArgs e) 
        {
            var context = (FlatViewModel)DataContext;
            if (context.HasOwner)
            {
                MessageBox.Show("Show user info");
            }
            else
            {
                EventRequestOwnerCreation(context);
            }
        }
    }
}