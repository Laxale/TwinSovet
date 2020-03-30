using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using NLog;


namespace Common.Helpers 
{
    /// <summary>
    /// Простая обёртка над классом <see cref="SendKeys"/>.
    /// </summary>
    public static class SendKeysHelper 
    {
        private static readonly string key = "{F8}";
        private static readonly object Locker = new object();
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private static volatile bool isActive;


        static SendKeysHelper() 
        {
            System.Windows.Application.Current.Activated += Current_OnActivated;
            System.Windows.Application.Current.Deactivated += Current_OnDeactivated;
        }


        /// <summary>
        /// Оживить класс.
        /// </summary>
        public static void Init() 
        {

        }

        /// <summary>
        /// Вызвать метод <see cref="SendKeys.SendWait"/>, если наше приложение сейчас активно (не нужно отправлять нажатие кнопки чужому приложению).
        /// </summary>
        public static void CallSendKeys() 
        {
            lock (Locker)
            {
                if (isActive)
                {
                    // Send валится в исключение "приложение не обрабатывает сообщения"
                    //SendKeys.Send(key);
                    // SendWait намертво вешает всю систему на виртуальной вин7
                    //SendKeys.SendWait(key);
                    Task.Run(() => SendKeys.SendWait(key))
                        .ContinueWith(task => logger.Info($"SendKeys done"));
                }
            }
        }


        private static void Current_OnActivated(object sender, EventArgs e) 
        {
            lock (Locker)
            {
                isActive = true;
            }
        }

        private static void Current_OnDeactivated(object sender, EventArgs e) 
        {
            lock (Locker)
            {
                isActive = false;
            }
        }
    }
}