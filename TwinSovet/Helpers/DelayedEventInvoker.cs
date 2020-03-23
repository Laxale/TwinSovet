using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;


namespace TwinSovet.Helpers 
{
    /// <summary>
    /// Позволяет получать событие через желаемый промежуток времени.
    /// </summary>
    public class DelayedEventInvoker : IDisposable 
    {
        private readonly Timer timer;

        private bool disposed;

        /// <summary>
        /// Минимальная задержка между запросом на событие и его вызовом.
        /// </summary>
        public const double MinimumEventDelayInMs = 10;

        /// <summary>
        /// Событие произошло - задержка между запросом на событие и вызовом события истекла.
        /// </summary>
        public event Action DelayedEvent = () => { };


        /// <summary>
        /// Конструирует <see cref="DelayedEventInvoker"/> с заданной задержкой перед вызовом события.
        /// </summary>
        /// <param name="eventDelayInMs">Задержка перед вызовом события в миллисекундах.</param>
        public DelayedEventInvoker(int eventDelayInMs)
        {
            timer = new Timer(eventDelayInMs < MinimumEventDelayInMs ? MinimumEventDelayInMs : eventDelayInMs);

            timer.Elapsed += (sender, args) =>
            {
                timer.Stop();
                try
                {
                    DelayedEvent();
                }
                catch (TaskCanceledException cancelEx)
                {
                    // так бывает когда при закрытии приложения отменяется выполнение отложенного события
                    Console.WriteLine(cancelEx);
                }
            };
        }

        ~DelayedEventInvoker()
        {
            Dispose();
        }


        /// <summary>
        /// Запросить отложенное событие. Перезапускает таймер, если предыдущий запрос в процессе ожидания.
        /// </summary>
        public void RequestDelayedEvent()
        {
            timer.Stop();
            timer.Start();
        }

        /// <summary>
        /// Отменить запрос на отложенное событие.
        /// </summary>
        public void RemoveDelayedEventRequest()
        {
            timer.Stop();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (disposed) return;
            timer.Dispose();

            disposed = true;
        }
    }
}
