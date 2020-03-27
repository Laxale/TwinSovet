using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using TwinSovet.Attributes;
using TwinSovet.Helpers;
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

            Loaded += OnLoaded;
        }


        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            NonDesignInvoker.Invoke(this, () =>
            {
                var binding = new Binding
                {
                    Mode = BindingMode.OneWay,
                    Path = new PropertyPath(nameof(SubjectNotesViewModel.NoteDecorators))
                };

                NotesList.SetBinding(ItemsControl.ItemsSourceProperty, binding);
            });
        }
    }
}