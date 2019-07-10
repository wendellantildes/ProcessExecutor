using System;
using ProcessExecutor.Domain.Processes;

namespace ProcessExecutor.Domain.Interfaces
{
    public interface IProcessRepository
    {
        void Add(Process process);

        void Update(Process process);

        Process Get(CurrentProcessSpecification specification);

        bool Has(CurrentProcessSpecification specification);
    }
}
