using System;
using System.Threading;
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
            CancellationTokenSource cts = new CancellationTokenSource();
            ExecutionManager executionManager  = null;
            Task.Run(() =>
            {
               executionManager = new ExecutionManager(cts);
            });
            cts.Token.Register(() =>
            {
                executionManager.Dispose();
            });
            Console.ReadLine();
            cts.Cancel();
            Console.WriteLine("Canceled");
            cts.Dispose();
        }
    }


    class ExecutionManager : IDisposable
    {
        private const string ListenerJobName = "ListenerJob";
        private const string ExecutorJobName = "ExecutorJob";
        private readonly CancellationTokenSource _cancellationTokenSource;

        public ExecutionManager(CancellationTokenSource cancellationTokenSource)
        {
            _cancellationTokenSource = cancellationTokenSource;
            JobManager.AddJob(new ListenerJob(StartExecutor), (s) =>
            {
                s.NonReentrant().WithName(ListenerJobName).ToRunEvery(5).Seconds();
            });
        }

        public void Dispose()
        {
            //ExecutorJob.Instance.RequestCancellation();
            JobManager.Stop();
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
