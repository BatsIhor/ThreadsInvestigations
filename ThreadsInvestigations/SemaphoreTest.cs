using System.Threading;

namespace ThreadsInvestigations
{
    public class SemaphoreTest
    {
        Semaphore s = new Semaphore(3, 3);

        public void TestSemaphore()
        {
            for (int i = 0; i < 10; i++)
                new Thread(Go).Start();
        }

        void Go()
        {
            while (true)
            {
                s.WaitOne();
                // Только 3 потока могут находиться здесь одновременно
                Thread.Sleep(100);
                s.Release();
            }
        }
    }
}
