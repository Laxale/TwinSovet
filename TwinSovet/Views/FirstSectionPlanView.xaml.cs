using System;
using System.Windows.Controls;

using TwinSovet.Attributes;
using TwinSovet.ViewModels;


namespace TwinSovet.Views 
{
    /// <summary>
    /// Логика взаимодействия для FirstSectionPlanView.xaml
    /// </summary>
    [HasViewModel(typeof(FirstSectionPlanViewModel))]
    public partial class FirstSectionPlanView : UserControl 
    {
        internal event Action<FlatViewModel> EventShowFlatDetails = flatModel => { };


        public FirstSectionPlanView() 
        {
            InitializeComponent();
        }


        private void FloorView_OnEventShowFlatDetails(FlatViewModel flatModel) 
        {
            EventShowFlatDetails(flatModel);
        }
    }
}