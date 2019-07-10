using System;
using System.Linq.Expressions;

namespace ProcessExecutor.Domain.Processes
{
    public class CurrentProcessSpecification
    {
        public CurrentProcessSpecification()
        {
            Criteria = (x) => x.Status != Status.Finished;
        }


        public Expression<Func<Process, bool>> Criteria { get; private set; }
    }
}
