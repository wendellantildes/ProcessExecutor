using System;
namespace ProcessExecutor.Domain.Processes
{
    public enum Status
    {
        Created  = 1,
        Started = 2,
        WithError = 3,
        Finished = 4
    }
}
