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
    /// Логика взаимодействия для NotePanelView.xaml
    /// </summary>
    public partial class NotePanelView : IDetailedAttachnemtView 
    {
        public event Action EventCancelRequest = () => { };


        public NotePanelView() 
        {
            InitializeComponent();

            CreationTime = DateTime.Now;
        }


        public DateTime CreationTime { get; }

        private NotePanelDecorator ViewModel => (NotePanelDecorator)DataContext;


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