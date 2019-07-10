using System;
namespace ProcessExecutor.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        bool Commit();
    }
}
