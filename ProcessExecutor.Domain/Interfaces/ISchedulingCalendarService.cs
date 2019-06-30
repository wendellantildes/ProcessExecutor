using System;
using System.Collections.Generic;

namespace ProcessExecutor.Domain.Interfaces
{
    public interface ISchedulingCalendarService
    {
        void Add(Scheduling scheduling, List<Scheduling> schedulings, DateTime? reference);
    }
}
