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


namespace TwinSovet.Views 
{
    /// <summary>
    /// Логика взаимодействия для EditAborigenView.xaml
    /// </summary>
    public partial class EditAborigenView : UserControl 
    {
        public event Action EventCancellationRequest = () => { };


        public EditAborigenView() 
        {
            InitializeComponent();
        }


        public void FocusInnerBox() 
        {
            DetailedAborigenCard.FocusInnerBox();
        }


        private void CancelAborigenDetailsButton_OnClick(object sender, RoutedEventArgs e) 
        {
            EventCancellationRequest();
        }

        private void EscapeCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e) 
        {
            EventCancellationRequest();
        }
    }
}