using System;
using System.Windows.Controls;

using TwinSovet.Attributes;
using TwinSovet.ViewModels;
using TwinSovet.ViewModels.Attachments;


namespace TwinSovet.Views 
{
    /// <summary>
    /// Interaction logic for SubjectNotesView.xaml
    /// </summary>
    [HasViewModel(typeof(SubjectNotesViewModel))]
    public partial class SubjectNotesView : UserControl 
    {
        public SubjectNotesView() 
        {
            InitializeComponent();
        }
    }
}