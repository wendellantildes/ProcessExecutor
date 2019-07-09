using System;
namespace ProcessExecutor.Domain.Processes
{
    public enum Step
    {
        LoadCredits =1,
        VerifyCredits = 2,
        LoadDebits = 3,
        VerifyDebits = 4,
        PaySalary = 5
    }
}
