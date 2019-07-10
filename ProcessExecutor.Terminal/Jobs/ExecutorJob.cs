using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentScheduler;
using Microsoft.Extensions.DependencyInjection;
using ProcessExecutor.Domain.Interfaces;
using ProcessExecutor.Terminal.Services.Interfaces;

namespace ProcessExecutor.Terminal.Jobs
{
    public class ExecutorJob : IJob
    {
        public static bool IsExecuting { private set; get; } = false;
        private static readonly object padlock = new object();

        //private ExecutorJob()
        //{
        //    IsExecuting = false;
        //}

        //private static ExecutorJob _instance;
        //public static ExecutorJob Instance
        //{
        //    get
        //    {
        //        lock (padlock)
        //        {
        //            if (_instance == null)
        //            {
        //                _instance = new ExecutorJob();
        //            }
        //        }
        //        return _instance;
        //    }
        //}

        private readonly ServiceProvider _serviceProvider;

        public ExecutorJob(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Execute()
        {
            try
            {
                lock (padlock)
                {
                    if (IsExecuting)
                    {
                        return;
                    }
                    IsExecuting = true;
                }


                using (var scope = _serviceProvider.CreateScope())
                {
                    var continuityService = scope.ServiceProvider.GetService<IContinuityService>();

                    //todo: test if it passes in finally block
                    if (!continuityService.CanResume())
                    {
                        return;
                    }

                    var defaultService = scope.ServiceProvider.GetService<IDefaultSalaryPaymentProcessService>();
                    defaultService.Start();
                    defaultService.LoadCredits();
                    defaultService.VerifyCredits();
                    defaultService.LoadDebits();
                    Task.Delay(10000).Wait();
                    defaultService.VerifyDebits();
                    defaultService.PaySalary();
                }

            }catch(Exception e)
            {

            }
            finally
            {
                lock (padlock)
                {
                    IsExecuting = false;
                }
            }
           
        }
    }
}
