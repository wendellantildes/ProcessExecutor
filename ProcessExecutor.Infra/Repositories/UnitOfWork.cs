using System;
using ProcessExecutor.Domain.Interfaces;
using ProcessExecutor.Infra.Repositories.Config;

namespace ProcessExecutor.Infra.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProcessExecutorContext _context;

        public UnitOfWork(ProcessExecutorContext context)
        {
            _context = context;
        }

        public bool Commit()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
