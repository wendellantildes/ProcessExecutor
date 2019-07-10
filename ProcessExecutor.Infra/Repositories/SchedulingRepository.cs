using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ProcessExecutor.Domain;
using ProcessExecutor.Domain.Interfaces;
using ProcessExecutor.Infra.Repositories.Config;
using System.Linq;

namespace ProcessExecutor.Infra.Repositories
{
    public class SchedulingRepository : ISchedulingRepository
    {
        private readonly ProcessExecutorContext _context;

        public SchedulingRepository(ProcessExecutorContext context)
        {
            _context = context;
        }

        public void Add(Scheduling scheduling)
        {
            throw new NotImplementedException();
        }

        public List<Scheduling> GetAllNotStarted()
        {
            return _context.Schedulings.Where(x => !x.Finished).ToList();
        }

        public void Update(Scheduling scheduling)
        {
            throw new NotImplementedException();
        }
    }
}
