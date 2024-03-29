﻿using System;
using System.IO;
using FluentScheduler;
using Microsoft.Extensions.DependencyInjection;
using ProcessExecutor.Domain.Interfaces;
using ProcessExecutor.Domain.Processes;
using ProcessExecutor.Terminal.Services.Interfaces;

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

                            var schedulingService = scope.ServiceProvider.GetService<ISchedulingService>();
                            var scheduling = schedulingService.Next();
                            if (scheduling == null)
                            {
                                return;
                            }
                            var date = scheduling.Date;
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
