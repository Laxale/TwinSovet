using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

using Common.Helpers;

using TwinSovet.Helpers;
using TwinSovet.Interfaces;
using TwinSovet.ViewModels.Attachments;


namespace TwinSovet.Views 
{
    /// <summary>
    /// Логика взаимодействия для AttachmentPanelView.xaml
    /// </summary>
    public partial class AttachmentPanelView : IDetailedAttachnemtView 
    {
        public event Action EventCancelRequest = () => { };


        public AttachmentPanelView() 
        {
            InitializeComponent();

            CreationTime = DateTime.Now;
        }


        public static readonly DependencyProperty TopSpecificContentTemplateProperty = 
            DependencyProperty.Register(nameof(TopSpecificContentTemplate), typeof(DataTemplate), 
                typeof(AttachmentPanelView), new PropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty BottomSpecificContentTemplateProperty =
            DependencyProperty.Register(nameof(BottomSpecificContentTemplate), typeof(DataTemplate),
                typeof(AttachmentPanelView), new PropertyMetadata(default(DataTemplate)));


        public DataTemplate TopSpecificContentTemplate
        {
            get => (DataTemplate)GetValue(TopSpecificContentTemplateProperty);
            set => SetValue(TopSpecificContentTemplateProperty, value);
        }

        public DataTemplate BottomSpecificContentTemplate 
        {
            get => (DataTemplate)GetValue(BottomSpecificContentTemplateProperty);
            set => SetValue(BottomSpecificContentTemplateProperty, value);
        }

        public DateTime CreationTime { get; }

        private AttachmentPanelDecoratorBase_NonGeneric ViewModel => (AttachmentPanelDecoratorBase_NonGeneric)DataContext;


        public void FocusInnerBox() 
        {
            DispatcherHelper.BeginInvokeOnDispatcher(() => Keyboard.Focus(TitleBox));
        }


        private void EscapeCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e) 
        {
            if (ViewModel.IsEditing)
            {
                ClientCommands.CommandCancelEditAttachment.Execute(ViewModel);
            }
            else
            {
                EventCancelRequest();
            }
        }


        private void ChildrenListBox_OnLoaded(object sender, RoutedEventArgs e) 
        {
            var binding = new Binding
            {
                Mode = BindingMode.OneWay,
                Path = new PropertyPath(nameof(NotePanelDecorator.Children))
            };

            ChildrenListBox.SetBinding(ItemsControl.ItemsSourceProperty, binding);
        }
    }
}