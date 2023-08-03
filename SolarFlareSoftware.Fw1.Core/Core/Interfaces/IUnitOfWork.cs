using System;

namespace SolarFlareSoftware.Fw1.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        bool BeginTransaction();
        bool BeginTransaction(IBaseRepository repository);
        bool Complete();
        void Rollback();
        bool JoinTransaction(IBaseRepository repository);
    }
}
