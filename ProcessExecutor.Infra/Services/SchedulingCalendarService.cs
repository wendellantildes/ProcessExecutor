using System;
using System.Collections.Generic;
using ProcessExecutor.Domain;
using ProcessExecutor.Domain.Interfaces;
using ProcessExecutor.Infra.Gateways;
using System.Linq;

namespace ProcessExecutor.Infra.Services
{
    public class SchedulingCalendarService : ISchedulingCalendarService
    {
        private IWeekDayGateway _weekDayGateway;
        private const int StartHour = 6;
        private const int EndHour = 18;

        public SchedulingCalendarService(IWeekDayGateway weekDayGateway)
        {
            _weekDayGateway = weekDayGateway;
        }

        public void Add(Scheduling scheduling, List<Scheduling> schedulings, DateTime? reference)
        {
            if (!reference.HasValue)
            {
                reference = DateTime.Now;
            }
            if(schedulings == null)
            {
                schedulings = new List<Scheduling>();
            }

            if (schedulings.Any(x=> x.Date == scheduling.Date))
            {
                throw new ArgumentException($"It is not allowed to repeat a scheduling");
            }

            var startHourTimeSpan = new TimeSpan(StartHour, 0, 0);
            var endHourTimeSpan = new TimeSpan(EndHour, 0, 0);
            if (scheduling.Date.TimeOfDay < startHourTimeSpan 
                || scheduling.Date.TimeOfDay > endHourTimeSpan)
            {
                throw new ArgumentException($"It is not allowed to schedule a date before {StartHour} neither after {EndHour}");
            }

            var dayOfWeek = scheduling.Date.DayOfWeek;
            if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
            {
                throw new ArgumentException("It is not allowed to schedule a date on Weekends");
            }

            if (!_weekDayGateway.IsAWeekDay(scheduling.Date))
            {
                throw new ArgumentException("It is not allowed to schedule a date different from a week day");
            }

            var allowedDate = reference.Value.AddMinutes(-1);
            if (allowedDate > scheduling.Date)
            {
                throw new ArgumentException("It is not allowed to schedule a date smaller than now");
            }

            schedulings.Add(scheduling);
        }
    }
}
