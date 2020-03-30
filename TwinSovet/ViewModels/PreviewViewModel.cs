using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;


namespace TwinSovet.ViewModels 
{
    internal class PreviewViewModel : NotifyPropertyChangedBase 
    {
        private ImageSource previewImageSource;


        public bool HasPreview => PreviewImageSource != null;

        /// <summary>
        /// Возвращает ссылку на источник превью фотографии.
        /// </summary>
        public ImageSource PreviewImageSource
        {
            get => previewImageSource;

            private set
            {
                if (previewImageSource == value) return;

                previewImageSource = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasPreview));
            }
        }


        /// <summary>
        /// Задать источник превью.
        /// </summary>
        /// <param name="previewSource">Источник превью картинки.</param>
        public void SetPreviewSource(ImageSource previewSource) 
        {
            PreviewImageSource = previewSource;
        }
    }
}