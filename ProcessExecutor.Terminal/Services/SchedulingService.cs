using System;
using ProcessExecutor.Domain;
using ProcessExecutor.Domain.Interfaces;
using ProcessExecutor.Terminal.Services.Interfaces;

namespace ProcessExecutor.Terminal.Services
{
    public class SchedulingService : ISchedulingService
    {
        private readonly ISchedulingRepository _schedulingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SchedulingService(IUnitOfWork unitOfWork, ISchedulingRepository schedulingRepository)
        {
            _schedulingRepository = schedulingRepository;
            _unitOfWork = unitOfWork;
        }

        public Scheduling Next()
        {
            var schedulings = _schedulingRepository.GetAllNotStarted();
            if(schedulings.Count == 0)
            {
                return null;
            }
            else
            {
                return schedulings[0];
            }
        }
    }
}
