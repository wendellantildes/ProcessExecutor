using System;
using System.Collections.Generic;

namespace ProcessExecutor.Domain.Processes.Interfaces
{
    public interface IVerifyDebitsService
    {
        void Verify(Process process, List<string> filesOfTheDay);
    }
}
