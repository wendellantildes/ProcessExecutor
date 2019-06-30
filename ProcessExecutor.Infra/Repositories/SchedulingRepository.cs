using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ProcessExecutor.Domain;
using ProcessExecutor.Domain.Interfaces;

namespace ProcessExecutor.Infra.Repositories
{
    public class SchedulingRepository : ISchedulingRepository
    {
        public void Add(Scheduling scheduling)
        {
            throw new NotImplementedException();
        }

        public List<Scheduling> GetAllNotStarted()
        {
            throw new NotImplementedException();
        }

        public void Update(Scheduling scheduling)
        {
            throw new NotImplementedException();
        }
    }
}
