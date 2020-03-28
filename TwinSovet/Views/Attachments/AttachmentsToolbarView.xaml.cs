using System;
using System.Windows;
using System.Windows.Controls;


namespace TwinSovet.Views.Attachments 
{
    /// <summary>
    /// Логика взаимодействия для AttachmentsToolbarView.xaml
    /// </summary>
    public partial class AttachmentsToolbarView : UserControl 
    {
        public event Action EventCreateNewAttachment = () => { };


        public AttachmentsToolbarView()
        {
            InitializeComponent();
        }


        private void NewAttachmentButton_OnClick(object sender, RoutedEventArgs e) 
        {
            EventCreateNewAttachment();
        }
    }
}