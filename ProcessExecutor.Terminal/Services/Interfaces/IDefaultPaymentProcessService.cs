using System;
namespace ProcessExecutor.Terminal.Services.Interfaces
{
    public interface IDefaultSalaryPaymentProcessService
    {
        void Start();

        void LoadCredits();

        void VerifyCredits();

        void LoadDebits();

        void VerifyDebits();

        void PaySalary();
    }
}
