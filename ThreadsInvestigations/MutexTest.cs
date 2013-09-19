using System;
using System.Threading;

namespace ThreadsInvestigations
{
    class MutexTest
    {
        // Используем уникальное имя приложения,
        // например, с добавлением имени компании
        Mutex mutex = new Mutex(false, "oreilly.com OneAtATimeDemo");

        public void TestMutex()
        {
            // Ожидаем получения мьютекса 5 сек – если уже есть запущенный
            // экземпляр приложения - завершаемся.
            if (!mutex.WaitOne(TimeSpan.FromSeconds(5), false))
            {
                Console.WriteLine("В системе запущен другой экземпляр программы!");
                return;
            }

            try
            {
                Console.WriteLine("Работаем - нажмите Enter для выхода...");
                Console.ReadLine();
            }
            finally { mutex.ReleaseMutex(); }
        }
    }
}
