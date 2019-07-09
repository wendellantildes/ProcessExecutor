using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentScheduler;

namespace ProcessExecutor.Terminal.Jobs
{
    public class ExecutorJob : IJob
    {
        public static bool IsExcuting { private set; get; }

        private ExecutorJob()
        {
            IsExcuting = false;
        }

        private static readonly object padlock = new object();
        private static ExecutorJob _instance;
        public static ExecutorJob Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new ExecutorJob();
                    }
                }
                return _instance;
            }
        }

        public void Execute()
        {
            lock (padlock)
            {
                if (IsExcuting)
                {
                    return;
                }
                IsExcuting = true;
            }
            Console.WriteLine($"[{DateTime.Now}-{nameof(ExecutorJob)}] Executor is starting...");
            Task.Delay(10000).Wait();
            Console.WriteLine($"[{DateTime.Now}-{nameof(ExecutorJob)}] Executor is running...");
            Task.Delay(10000).Wait();
            Console.WriteLine($"[{DateTime.Now}-{nameof(ExecutorJob)}] Executor is running...");
            Task.Delay(10000).Wait();
            lock (padlock)
            {
                IsExcuting = false;
            }
        }
    }
}
