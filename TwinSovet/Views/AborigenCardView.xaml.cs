using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace TwinSovet.Views
{
    /// <summary>
    /// Логика взаимодействия для AborigenCardView.xaml
    /// </summary>
    public partial class AborigenCardView : UserControl 
    {
        public AborigenCardView() 
        {
            InitializeComponent();
        }


        public void FocusInnerBox() 
        {
            Keyboard.Focus(NameBox);
        }
    }
}