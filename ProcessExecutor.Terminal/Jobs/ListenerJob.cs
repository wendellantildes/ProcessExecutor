using System;
using System.IO;
using FluentScheduler;

namespace ProcessExecutor.Terminal.Jobs
{
    public class ListenerJob : IJob
    {
        private const string SchedulerSource = "scheduler.txt";
        public delegate void StartExecutor();
        private StartExecutor _action;
        private DateTime _lastExecution = DateTime.MinValue;

        public ListenerJob(StartExecutor action)
        {
            _action = action;
        }

        public void Execute()
        {
            var now = DateTime.Now;
            if (ExecutorJob.IsExcuting)
            {
                Console.WriteLine($"[{now}-{nameof(ListenerJob)}] There is already an Executor running");
            }
            else
            {
                Console.WriteLine($"[{now}-{nameof(ListenerJob)}] Listening...");
                if (File.Exists(SchedulerSource))
                {
                    var schedule = File.ReadAllText(SchedulerSource);
                    var date = DateTime.Parse(schedule);
                    if(date <= now && date >= _lastExecution)
                    {
                        Console.WriteLine($"[{now}-{nameof(ListenerJob)}] Last exection at {_lastExecution}");
                        _lastExecution = now;
                        _action();
                    }
                }
            }
        }
    }
}
