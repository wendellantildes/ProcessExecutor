using System;
using System.Collections.Generic;

namespace ProcessExecutor.Domain.Interfaces
{
    public interface ISchedulingRepository
    {
        void Add(Scheduling scheduling);

        void Update(Scheduling scheduling);

        List<Scheduling> GetAllNotStarted();
    }
}
