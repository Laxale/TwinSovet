using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Common.Helpers;
using TwinSovet.Helpers;
using TwinSovet.Interfaces;
using TwinSovet.ViewModels.Attachments;


namespace TwinSovet.Views.Attachments 
{
    /// <summary>
    /// Interaction logic for CreateAttachmentView.xaml
    /// </summary>
    public partial class CreateAttachmentView : UserControl, IDetailedAttachnemtView 
    {
        public event Action EventCancelRequest = () => { };


        public CreateAttachmentView() 
        {
            InitializeComponent();

            CreationTime = DateTime.Now;
        }


        public static readonly DependencyProperty SpecificContentTemplateProperty = 
            DependencyProperty.Register(nameof(SpecificContentTemplate), typeof(DataTemplate), 
                typeof(CreateAttachmentView), new PropertyMetadata(default(object)));


        public DataTemplate SpecificContentTemplate 
        {
            get => (DataTemplate)GetValue(SpecificContentTemplateProperty);
            set => SetValue(SpecificContentTemplateProperty, value);
        }

        public DateTime CreationTime { get; }


        private AttachmentViewModelBase ViewModel => (AttachmentViewModelBase)DataContext;


        public void FocusInnerBox() 
        {
            DispatcherHelper.BeginInvokeOnDispatcher(() => Keyboard.Focus(TitleBox));
        }


        private void CancelCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e) 
        {
            EventCancelRequest();
        }


        private void EscapeButton_OnClick(object sender, RoutedEventArgs e) 
        {
            EventCancelRequest();
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