using System;
using ProcessExecutor.Domain;

namespace ProcessExecutor.Terminal.Services.Interfaces
{
    public interface ISchedulingService
    {
        Scheduling Next();
    }
}
