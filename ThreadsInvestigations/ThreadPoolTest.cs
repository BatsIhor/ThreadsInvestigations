using System;
using System.Threading;

namespace ThreadsInvestigations
{
    public class ThreadPoolTest
    {
        private ManualResetEvent starter = new ManualResetEvent(false);

        public void Do()
        {
            ThreadPool.RegisterWaitForSingleObject(starter, Go, "привет", -1, true);
            Thread.Sleep(5000);
            Console.WriteLine("Запускается рабочий поток...");
            starter.Set();
            Console.ReadLine();
        }

        public void Go(object data, bool timedOut)
        {
            Console.WriteLine("Запущено: " + data);
            // Выполнение задачи...
        }
    }

    //---------------------------------------------------------------

    public class ThreadPoolTest1
    {
        object workerLocker = new object();
        int runningWorkers = 100;

        public void Do1()
        {
            for (int i = 0; i < runningWorkers; i++)
                ThreadPool.QueueUserWorkItem(Go, i);

            Console.WriteLine("Ожидаем завершения работы потоков...");

            lock (workerLocker)
                while (runningWorkers > 0)
                    Monitor.Wait(workerLocker);

            Console.WriteLine("Готово!");
            Console.ReadLine();
        }

        public void Go(object instance)
        {
            Console.WriteLine("Запущен:  " + instance);
            Thread.Sleep(1000);
            Console.WriteLine("Завершен: " + instance);

            lock (workerLocker)
            {
                runningWorkers--;
                Monitor.Pulse(workerLocker);
            }
        }
    }

}
