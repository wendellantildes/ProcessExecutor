using System;
using System.IO;
using FluentScheduler;
using Microsoft.Extensions.DependencyInjection;
using ProcessExecutor.Domain.Interfaces;
using ProcessExecutor.Domain.Processes;

namespace ProcessExecutor.Terminal.Jobs
{
    public class ListenerJob : IJob
    {
        private const string SchedulerSource = "scheduler.txt";
        public delegate void StartExecutor();
        private StartExecutor _action;
        private DateTime _lastExecution = DateTime.MinValue;
        private readonly ServiceProvider _serviceProvider;

        public ListenerJob(ServiceProvider serviceProvider, StartExecutor action)
        {
            _action = action;
            _serviceProvider = serviceProvider;
        }

        public void Execute()
        {
            var now = DateTime.Now;
            if (ExecutorJob.IsExecuting)
            {
                Console.WriteLine($"[{now}-{nameof(ListenerJob)}] There is already an Executor running");
            }
            else
            {
                using (var scope = _serviceProvider.CreateScope())
                {


                    try
                    {
                        var processRepository = scope.ServiceProvider.GetService<IProcessRepository>();
                        if (processRepository.Has(new CurrentProcessSpecification()))
                        {
                            _action();
                        }
                        else
                        {
                            Console.WriteLine($"[{now}-{nameof(ListenerJob)}] Listening...");

                            var schedulingRepository = scope.ServiceProvider.GetService<ISchedulingRepository>();
                            //todo: extract to a service
                            var schedulings = schedulingRepository.GetAllNotStarted();
                            if (schedulings.Count == 0)
                            {
                                return;
                            }
                            var first = schedulings[0];
                            var date = first.Date;
                            if (date <= now && date >= _lastExecution)
                            {
                                Console.WriteLine($"[{now}-{nameof(ListenerJob)}] Last exection at {_lastExecution}");
                                _lastExecution = now;
                                _action();
                            }

                            //if (File.Exists(SchedulerSource))
                            //{
                            //    var schedule = File.ReadAllText(SchedulerSource);
                            //    var date = DateTime.Parse(schedule);
                            //    if (date <= now && date >= _lastExecution)
                            //    {
                            //        Console.WriteLine($"[{now}-{nameof(ListenerJob)}] Last exection at {_lastExecution}");
                            //        _lastExecution = now;
                            //        _action();
                            //    }
                            //}
                        }
                    }
                    catch (Exception e)
                    {

                    }

                }


            }
        }
    }
}
