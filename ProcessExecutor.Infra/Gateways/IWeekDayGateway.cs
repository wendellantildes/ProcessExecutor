using System;
namespace ProcessExecutor.Infra.Gateways
{

    public interface IWeekDayGateway
    {
        bool IsAWeekDay(DateTime date);
    }
}
