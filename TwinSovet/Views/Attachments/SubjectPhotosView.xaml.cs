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

using TwinSovet.Attributes;
using TwinSovet.Data.Models.Attachments;
using TwinSovet.Helpers.Attachments;
using TwinSovet.ViewModels.Attachments;


namespace TwinSovet.Views.Attachments 
{
    /// <summary>
    /// Interaction logic for SubjectPhotosView.xaml
    /// </summary>
    [HasViewModel(typeof(SubjectPhotosViewModel))]
    public partial class SubjectPhotosView : UserControl 
    {
        private readonly CommonSubjectAttachmentsLogic<PhotoAlbumAttachmentModel, PhotoAlbumAttachmentViewModel> logicHolder;


        public SubjectPhotosView() 
        {
            InitializeComponent();

            logicHolder = new CommonSubjectAttachmentsLogic<PhotoAlbumAttachmentModel, PhotoAlbumAttachmentViewModel>
            (
                this,
                () => RootGrid,
                () => PhotosList,
                () => IsAddingNew,
                value => IsAddingNew = value,
                () => ViewModel,
                CreateAttachmentViewModel
            );

            Loaded += OnLoaded;
        }


        public static readonly DependencyProperty IsAddingNewProperty =
            DependencyProperty.Register(nameof(IsAddingNew), typeof(bool), typeof(SubjectPhotosView), new PropertyMetadata(default(bool)));


        /// <summary>
        /// 
        /// </summary>
        public bool IsAddingNew 
        {
            get => (bool)GetValue(IsAddingNewProperty);
            set => SetValue(IsAddingNewProperty, value);
        }


        private SubjectPhotosViewModel ViewModel { get; set; }


        private void OnLoaded(object sender, RoutedEventArgs e) 
        {
            ViewModel = (SubjectPhotosViewModel)DataContext;

            logicHolder.OnLoaded();
        }

        private void NewCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e) 
        {
            logicHolder.BeginAddNewAttachment();
        }

        private void NewCommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = logicHolder.CanExecuteNewCommandBinding();
        }


        private void AttachmentsToolbarView_OnEventCreateNewAttachment() 
        {
            logicHolder.BeginAddNewAttachment();
        }


        private PhotoAlbumAttachmentViewModel CreateAttachmentViewModel(AttachmentModelBase model) 
        {
            var albumViewModel = new PhotoAlbumAttachmentViewModel((PhotoAlbumAttachmentModel)model, false);
            albumViewModel.SetOwnerSubject(ViewModel.CurrentNotesOwner);

            return albumViewModel;
        }
    }
}