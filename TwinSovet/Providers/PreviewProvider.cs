using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TwinSovet.Data.Models.Attachments;
using TwinSovet.Data.Providers;


namespace TwinSovet.Providers 
{
    internal static class PreviewProvider 
    {
        private static readonly object Locker = new object();
        private static readonly string previewFolderName = "Previews";
        private static readonly string previewFolderPath = Path.Combine(StaticsProvider.InAppDataFolderPath, previewFolderName);
        private static readonly Dictionary<string, ImageSource> previewSources = new Dictionary<string, ImageSource>();


        static PreviewProvider() 
        {
            RemovePreviewDirectory();
            CreatePreviewFolder();

            Application.Current.Exit += Application_OnExit;
        }


        /// <summary>
        /// Сохранить в кэш данные превью фотографии.
        /// </summary>
        public static void SetPreview(PhotoAttachmentModel photoModel) 
        {
            lock (Locker)
            {
                if (previewSources.ContainsKey(photoModel.Id)) return;

                string previewFilePath = Path.Combine(previewFolderPath, $"preview_{photoModel.Id}");
                File.WriteAllBytes(previewFilePath, photoModel.PreviewDataBlob);

                previewSources.Add(photoModel.Id, new BitmapImage(new Uri(previewFilePath, UriKind.Absolute)));
            }
        }

        public static ImageSource GetPreview(string photoId) 
        {
            lock (Locker)
            {
                if (previewSources.TryGetValue(photoId, out ImageSource source))
                {
                    return source;
                }

                throw new InvalidOperationException($"Превью для идентификатора '{ photoId }' не существует");
            }
        }

        public static bool IsImage(string filePath) 
        {
            try
            {
                var source = new BitmapImage(new Uri(filePath));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }


        private static void RemovePreviewDirectory() 
        {
            if (Directory.Exists(previewFolderPath))
            {
                Directory.Delete(previewFolderPath, true);
            }
        }

        private static void CreatePreviewFolder() 
        {
            if (!Directory.Exists(previewFolderPath))
            {
                Directory.CreateDirectory(previewFolderPath);
            }
        }


        private static void Application_OnExit(object sender, ExitEventArgs e) 
        {
            RemovePreviewDirectory();
        }
    }
}