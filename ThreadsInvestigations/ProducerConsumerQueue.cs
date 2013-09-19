using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreadsInvestigations
{
    public class ProducerConsumerQueue : IDisposable
    {
        EventWaitHandle wh = new AutoResetEvent(false);
        Thread worker;
        object locker = new object();
        Queue<string> taskQueue = new Queue<string>();

        public ProducerConsumerQueue()
        {
            worker = new Thread(Go);
            worker.Start();
        }

        public void EnqueueTask(string task)
        {
            lock (locker)
                taskQueue.Enqueue(task);
            
            wh.Set();
        }

        public void Go()
        {
            while (true)
            {
                string task = null;
                wh.WaitOne();
                lock (locker)
                {
                    if (taskQueue.Count > 0)
                    {
                        task = taskQueue.Dequeue();
                        if (task == null)
                        {
                            return;
                        }
                    }
                }
                if (task != null)
                {
                    Console.WriteLine(task);
                }
                else
                {
                    Console.ReadLine();
                }
            }
        }

        public void Dispose()
        {
            EnqueueTask(null);
            worker.Join();
            wh.Close();
        }
    }
}
