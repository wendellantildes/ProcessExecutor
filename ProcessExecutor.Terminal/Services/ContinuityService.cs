using System;
using ProcessExecutor.Domain.Interfaces;
using ProcessExecutor.Domain.Processes;
using ProcessExecutor.Terminal.Services.Interfaces;

namespace ProcessExecutor.Terminal.Services
{
    public class ContinuityService : IContinuityService
    {
        private readonly IProcessRepository _processRepository;

        public ContinuityService(IProcessRepository processRepository)
        {
            _processRepository = processRepository;
        }

        public bool CanResume()
        {
            var process = _processRepository.Get(new CurrentProcessSpecification());
            if(process == null)
            {
                return true;
            }
            else
            {
                return (process.Status == Status.Created || process.Status == Status.Started);
            }
        }
    }
}
