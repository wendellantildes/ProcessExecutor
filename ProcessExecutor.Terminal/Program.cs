using System;
using System.Threading.Tasks;
using FluentScheduler;
using ProcessExecutor.Terminal.Jobs;

namespace ProcessExecutor.Terminal
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Process Executor");
            Task.Run(() =>
            {
                var executionManager = new ExecutionManager();

            });
            Console.ReadLine();
        }
    }


    class ExecutionManager 
    {
        private const string ListenerJobName = "ListenerJob";
        private const string ExecutorJobName = "ExecutorJob";


        public ExecutionManager()
        {
            JobManager.AddJob(new ListenerJob(StartExecutor), (s) =>
            {
                s.NonReentrant().WithName(ListenerJobName).ToRunEvery(5).Seconds();
            });
        }

        public void StartExecutor()
        {
            JobManager.AddJob(ExecutorJob.Instance, (s) =>
            {
                s.NonReentrant().WithName(ExecutorJobName).ToRunNow();
            });
        }
    }
}
