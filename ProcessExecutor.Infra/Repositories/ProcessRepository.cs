using System;
using ProcessExecutor.Domain.Interfaces;
using ProcessExecutor.Domain.Processes;
using ProcessExecutor.Infra.Repositories.Config;
using System.Linq;

namespace ProcessExecutor.Infra.Repositories
{
    public class ProcessRepository : IProcessRepository
    {
        private readonly ProcessExecutorContext _context;

        public ProcessRepository(ProcessExecutorContext context)
        {
            _context = context;
        }

        public void Add(Process process)
        {
            _context.Processes.Add(process);
        }

        public void Update(Process process)
        {
            _context.Processes.Update(process);
        }

        public Process Get(CurrentProcessSpecification specification)
        {
            return _context.Processes.SingleOrDefault(specification.Criteria);
        }

        public bool Has(CurrentProcessSpecification specification)
        {
            return _context.Processes.Any(specification.Criteria);
        }
    }
}
