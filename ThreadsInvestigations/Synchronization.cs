using System;
using System.Runtime.Remoting.Contexts;
using System.Threading;

namespace ThreadsInvestigations
{
    [Synchronization]
    public class AutoLock : ContextBoundObject
    {
        public static string item;                      //it's out of context ?

        public void Demo()
        {
            Console.Write("Старт...");
            Thread.Sleep(1000);                         // Поток не может быть вытеснен здесь
            Console.WriteLine("стоп");                  // спасибо автоматической блокировке!
        }
    }

    public class Test
    {
        public static void Do()
        {
            AutoLock safeInstance = new AutoLock();     //safeInstance is a synchronization context.
            new Thread(safeInstance.Demo).Start();      // Запустить метод 
            new Thread(safeInstance.Demo).Start();      // Demo 3 раза
            safeInstance.Demo();                        // одновременно.
        }
    }
}
