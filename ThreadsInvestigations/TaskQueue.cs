using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreadsInvestigations
{
    public class TaskQueue : IDisposable
    {
        private object locker = new object();
        private Thread[] workers;
        private Queue<string> taskQ = new Queue<string>();

        public TaskQueue(int workerCount)
        {
            workers = new Thread[workerCount];

            // Создать и запустить отдельный поток на каждого потребителя
            for (int i = 0; i < workerCount; i++)
                (workers[i] = new Thread(Consume)).Start();
        }

        public void Dispose()
        {
            // Добавить по null-задаче на каждого завершаемого потребителя
            foreach (Thread worker in workers)
                EnqueueTask(null);

            foreach (Thread worker in workers)
                worker.Join();
        }

        public void EnqueueTask(string task)
        {
            lock (locker)
            {
                taskQ.Enqueue(task);
                Console.WriteLine("+");
                Monitor.PulseAll(locker);
            }
        }

        private void Consume()
        {
            while (true)
            {
                string task;

                lock (locker)
                {
                    while (taskQ.Count == 0)
                    {
                        Monitor.Wait(locker); //three threads are waiting here. When we PulseAll it goes WHILE again and if we have some tasks dequeue it .
                        //Next waiting thread will check it too only after current thread leavs LOCK in case if no more tasks there it will hold again.
                    }

                    task = taskQ.Dequeue();
                }

                if (task == null)
                    return; // Сигнал на выход

                Console.WriteLine(task);
                Thread.Sleep(1000); // Имитация длительной работы
            }
        }
    }
}