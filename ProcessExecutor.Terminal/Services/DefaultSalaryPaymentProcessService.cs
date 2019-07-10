using System;
using ProcessExecutor.Domain.Interfaces;
using ProcessExecutor.Domain.Processes;
using ProcessExecutor.Terminal.Services.Interfaces;

namespace ProcessExecutor.Terminal.Services
{
    public class DefaultSalaryPaymentProcessService : IDefaultSalaryPaymentProcessService
    {
        private readonly IProcessRepository _processRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DefaultSalaryPaymentProcessService(IUnitOfWork unitOfWork, IProcessRepository processRepository)
        {
            _processRepository = processRepository;
            _unitOfWork = unitOfWork;
        }

        public void Start()
        {
            var process = _processRepository.Get(new CurrentProcessSpecification());
            if (process == null)
            {
                process = Process.NewDefaultSalaryPaymentProcess();
            }

            if (process.Status == Status.Created)
            {
                process.Start();
                _processRepository.Add(process);
                _unitOfWork.Commit();
            }
        }

        public void LoadCredits()
        {
            var process = _processRepository.Get(new CurrentProcessSpecification());
            if (process != null)
            {
                var tasks = process.TasksFromStep(Step.LoadCredits);
                foreach (var task in tasks)
                {
                    process.Start(task);
                    process.Finish(task);
                }
                _processRepository.Update(process);
                _unitOfWork.Commit();
            }
        }

        public void VerifyCredits()
        {
            var process = _processRepository.Get(new CurrentProcessSpecification());
            if (process != null)
            {
                var tasks = process.TasksFromStep(Step.VerifyCredits);
                foreach (var task in tasks)
                {
                    process.Start(task);
                    process.Finish(task);
                }
                _processRepository.Update(process);
                _unitOfWork.Commit();
            }
        }

        public void LoadDebits()
        {
            var process = _processRepository.Get(new CurrentProcessSpecification());
            if (process != null)
            {
                var tasks = process.TasksFromStep(Step.LoadDebits);
                foreach (var task in tasks)
                {
                    process.Start(task);
                    process.Finish(task);
                }
                _processRepository.Update(process);
                _unitOfWork.Commit();
            }
        }

        public void VerifyDebits()
        {
            var process = _processRepository.Get(new CurrentProcessSpecification());
            if (process != null)
            {
                var tasks = process.TasksFromStep(Step.VerifyDebits);
                foreach (var task in tasks)
                {
                    process.Start(task);
                    process.Finish(task);
                }
                _processRepository.Update(process);
                _unitOfWork.Commit();
            }
        }

        public void PaySalary()
        {
            var process = _processRepository.Get(new CurrentProcessSpecification());
            if (process != null)
            {
                var tasks = process.TasksFromStep(Step.PaySalary);
                foreach (var task in tasks)
                {
                    process.Start(task);
                    process.Finish(task);
                }
                process.Finish();
                _processRepository.Update(process);
                _unitOfWork.Commit();
            }
        }
    }
}
