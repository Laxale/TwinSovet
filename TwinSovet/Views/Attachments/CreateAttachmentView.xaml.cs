using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using TwinSovet.Helpers;
using TwinSovet.ViewModels.Attachments;


namespace TwinSovet.Views.Attachments 
{
    /// <summary>
    /// Interaction logic for CreateAttachmentView.xaml
    /// </summary>
    public partial class CreateAttachmentView : UserControl 
    {
        public event Action EventCancelCreation = () => { };


        public CreateAttachmentView() 
        {
            InitializeComponent();
        }


        public static readonly DependencyProperty SpecificAttachmentContentProperty = 
            DependencyProperty.Register(nameof(SpecificAttachmentContent), typeof(object), 
                typeof(CreateAttachmentView), new PropertyMetadata(default(object)));


        public object SpecificAttachmentContent 
        {
            get => (object)GetValue(SpecificAttachmentContentProperty);
            set => SetValue(SpecificAttachmentContentProperty, value);
        }


        private AttachmentViewModelBase ViewModel => (AttachmentViewModelBase)DataContext;


        private void CancelCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e) 
        {
            EventCancelCreation();
        }


        private void EscapeButton_OnClick(object sender, RoutedEventArgs e) 
        {
            EventCancelCreation();
        }

        private void AcceptCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ClientCommands.CommanSaveAttachment.Execute(ViewModel);

            e.Handled = true;
        }

        private void AcceptCommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e) 
        {
            e.CanExecute = ClientCommands.CommanSaveAttachment.CanExecute(ViewModel);
        }
    }
}