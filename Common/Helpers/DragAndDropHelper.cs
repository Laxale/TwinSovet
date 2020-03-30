using System;
using System.IO;
using System.Linq;
using System.Windows;


namespace Common.Helpers 
{
    /// <summary>
    /// Содержит вспомогательные функции для рабоыт с событием Drag-and-Drop.
    /// </summary>
    public class DragAndDropHelper : IDisposable 
    {
        private readonly bool simulateClick;
        private readonly DependencyObject owner;


        /// <summary>
        /// Конструирует <see cref="DragAndDropHelper"/>.
        /// </summary>
        /// <param name="owner">Элемент, осуществляющий drag-and-drop.</param>
        /// <param name="simulateClick">Симулировать ли нажатие Enter в окне приложения (может быть нужно для возврата фокуса в него).</param>
        public DragAndDropHelper(DependencyObject owner, bool simulateClick)
        {
            this.owner = owner;
            this.simulateClick = simulateClick;
        }


        /// <summary>
        /// Получить список путей перетащенных файлов из <see cref="DragEventArgs"/>.
        /// </summary>
        /// <param name="dragEventArgs">Аргументы события перетаскивания.</param>
        /// <returns>Список файлов, которые были drag-and-dropнуты юзером.</returns>
        public string[] GetDroppedFiles(DragEventArgs dragEventArgs) 
        {
            string[] droppedFilePaths = dragEventArgs.Data.GetData("FileDrop") as string[];
            string[] droppedFiles = droppedFilePaths?.Where(File.Exists).ToArray() ?? new string[] { };

            return droppedFiles;
        }


        /// <summary>
        /// Выполняет активацию родительского окна элемента owner, фокус которого зависает после выполнения drag-and-drop.
        /// </summary>
        public void Dispose() 
        {
            //после дропа файлов фокус остаётся вне окна, поэтому нужно его активировать
            var ownerWindow = owner is Window window ? window : Window.GetWindow(owner);

            ownerWindow?.Activate();

            // эта штука симулирует юзер инпут, иначе содержимое окна не обновляется, пока не кликнешь по нему
            if (simulateClick)
            {
                SendKeysHelper.CallSendKeys();
                // ентер слишком используемая кнопка. порой он тут выполняет действия неожиданно для юзера
                //SendKeys.SendWait("{ENTER}");
            }
        }
    }
}
