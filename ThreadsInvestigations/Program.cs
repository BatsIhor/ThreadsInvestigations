using System;
using System.Threading;

namespace ThreadsInvestigations
{
    internal class Program
    {
        //static bool ready;
        //static object locker = new object();

        //---------------------------------------------------------------------------------
        //static void Main()
        //{
        //    var t = new Thread(() => Console.WriteLine("d"));

        //    t.Start();
        //    //t.Join(); sync without lock
        //    Go();
        //}

        //static void Go()
        //{
        //    //lock (locker)
        //    //{
        //    if (!ready)
        //    {
        //        Console.WriteLine("Done");
        //        ready = true;
        //    }
        //    //}
        //}

        //---------------------------------------------------------------------------------
        //static void Main()
        //{
        //    Thread t = new Thread(() => MethodName("Hello"));
        //    t.Start();
        //}

        //static void MethodName(string text) { Console.WriteLine(text); }

        //---------------------------------------------------------------------------------
        //static EventWaitHandle wh = new AutoResetEvent(false);

        //static void Main()
        //{
        //    new Thread(Waiter).Start();
        //    Thread.Sleep(1000);                 // Подождать некоторое время...
        //    wh.Set();
        //    Console.ReadLine();
        //    // OK – можно разбудить
        //}

        //static void Waiter()
        //{
        //    Console.WriteLine("Waiting for signal");
        //    wh.WaitOne();                        // Ожидать сигнала
        //    Console.WriteLine("I get it!");
        //    Console.WriteLine("now I am doing something important");
        //}

        //static EventWaitHandle ready = new AutoResetEvent(false);
        //static EventWaitHandle go = new AutoResetEvent(false);
        //static volatile string task;

        //static void Main()
        //{
        //    new Thread(Work).Start();

        //    // Сигнализируем рабочему потоку 5 раз
        //    for (int i = 1; i <= 5; i++)
        //    {
        //        Console.WriteLine("ready.WaitOne()");
        //        ready.WaitOne();                // Сначала ждем, когда рабочий поток будет готов
        //        task = "a".PadRight(i, 'h');    // Назначаем задачу

        //        Console.WriteLine("go.Set()");
        //        go.Set();                       // Говорим рабочему потоку, что можно начинать
        //    }

        //    Console.ReadLine();
        //    // Сообщаем о необходимости завершения рабочего потока, используя null-строку
        //    ready.WaitOne();
        //    task = null;
        //    go.Set();

        //    Console.ReadLine();
        //}

        //static void Work()
        //{
        //    while (true)
        //    {
        //        Console.WriteLine("ready.Set()");
        //        ready.Set();                  // Сообщаем о готовности

        //        Console.WriteLine("go.WaitOne()");
        //        go.WaitOne();                 // Ожидаем сигнала начать...

        //        if (task == null)
        //            return;                     // Элегантно завершаемся

        //        Console.WriteLine(task);
        //    }
        //}

        private static AutoResetEvent task = new AutoResetEvent(false);
        private static AutoResetEvent ready = new AutoResetEvent(false);

        private static string taskString = string.Empty;

        private static bool stop;

        private static void Main()
        {
            //TestProducerConsumerQueue();
            // ThreadPoolTest1 threadPoolTest1 = new ThreadPoolTest1();
            //threadPoolTest1.Do1();

            TaskQueue taskQueue = new TaskQueue(3);

            Thread.Sleep(1500);
            for (int i = 0; i < 10; i++)
            {
                taskQueue.EnqueueTask(i.ToString());
            }
        }

        private static void TestProducerConsumerQueue()
        {
            new Thread(SomeWork).Start();

            for (int i = 0; i < 5; i++)
            {
                taskString += "A";
                task.Set();
                ready.WaitOne();
            }

            using (var q = new ProducerConsumerQueue())
            {
                q.EnqueueTask("Привет!");

                for (int i = 0; i < 10; i++)
                    q.EnqueueTask("Сообщение " + i);

                q.EnqueueTask("Пока!");
            }
        }

        public static void SomeWork()
        {
            while (true)
            {
                task.WaitOne();
                Console.WriteLine(taskString);
                ready.Set();
            }
        }
    }
}
