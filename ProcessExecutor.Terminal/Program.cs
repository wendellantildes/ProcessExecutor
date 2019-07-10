using System;
using System.Threading;
using System.Threading.Tasks;
using FluentScheduler;
using Microsoft.Extensions.DependencyInjection;
using ProcessExecutor.Domain.Interfaces;
using ProcessExecutor.Terminal.Jobs;
using ProcessExecutor.Infra.Repositories;
using ProcessExecutor.Infra.Repositories.Config;
using ProcessExecutor.Terminal.Services.Interfaces;
using ProcessExecutor.Terminal.Services;

namespace ProcessExecutor.Terminal
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Process Executor");
            var startup = new Startup();
            var serviceCollection = new ServiceCollection();
            startup.Configure(serviceCollection);
            CancellationTokenSource cts = new CancellationTokenSource();
            ExecutionManager executionManager  = null;
            Task.Run(() =>
            {
               executionManager = new ExecutionManager(serviceCollection.BuildServiceProvider(), cts);
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

    public class Startup
    {
        public void Configure(IServiceCollection services)
        {
            services.AddDbContext<ProcessExecutorContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDefaultSalaryPaymentProcessService, DefaultSalaryPaymentProcessService>();
            services.AddScoped<ISchedulingService, SchedulingService>();
            services.AddScoped<IContinuityService, ContinuityService>();
            services.AddScoped<ISchedulingRepository, SchedulingRepository>();
            services.AddScoped<IProcessRepository, ProcessRepository>();
            services.AddScoped<ISchedulingRepository, SchedulingRepository>();
        }
    }


    class ExecutionManager : IDisposable
    {
        private const string ListenerJobName = "ListenerJob";
        private const string ExecutorJobName = "ExecutorJob";
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly ServiceProvider _serviceProvider;
        public ExecutionManager(ServiceProvider serviceProvider, CancellationTokenSource cancellationTokenSource)
        {
            _serviceProvider = serviceProvider;
            //_cancellationTokenSource = cancellationTokenSource;
            JobManager.AddJob(new ListenerJob(_serviceProvider, StartExecutor), (s) =>
            {
                s.NonReentrant().WithName(ListenerJobName).ToRunEvery(10).Seconds();
               // s.NonReentrant().WithName(ListenerJobName).ToRunNow();
            });
        }

        public void Dispose()
        {
            //ExecutorJob.Instance.RequestCancellation();
            JobManager.Stop();
        }

        public void StartExecutor()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                JobManager.AddJob(new ExecutorJob(_serviceProvider), (s) =>
                {
                    s.NonReentrant().WithName(ExecutorJobName).ToRunNow();
                });
            }
        }
    }
}
