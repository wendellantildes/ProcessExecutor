using System;
using Xunit;
using ProcessExecutor.Infra.Services;
using NSubstitute;
using ProcessExecutor.Infra.Gateways;
using ProcessExecutor.Domain;
using System.Collections.Generic;

namespace ProcessExecutor.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void AddFutureScheduling_Success()
        {
            //arrange
            var weekDayGateway = Substitute.For<IWeekDayGateway>();
            weekDayGateway.IsAWeekDay(Arg.Any<DateTime>()).Returns(true);
            var service = new SchedulingCalendarService(weekDayGateway);
            var schedulings = new List<Scheduling>();
            var scheduling = new Scheduling(ReferenceDate().AddMinutes(1));

            //act
            service.Add(scheduling, schedulings, ReferenceDate());

            //assert
            Assert.True(schedulings.Count > 0);
        }

        [Fact]
        public void AddImmediateSchedulingTo_Success()
        {
            //arrange
            var weekDayGateway = Substitute.For<IWeekDayGateway>();
            weekDayGateway.IsAWeekDay(Arg.Any<DateTime>()).Returns(true);
            var service = new SchedulingCalendarService(weekDayGateway);
            var schedulings = new List<Scheduling>();
            var scheduling = new Scheduling(ReferenceDate());

            //act
            service.Add(scheduling, schedulings, ReferenceDate());

            //assert
            Assert.True(schedulings.Count > 0);
        }

        [Fact]
        public void AddSchedulingOnSunday_Error()
        {
            //arrange
            var weekDayGateway = Substitute.For<IWeekDayGateway>();
            weekDayGateway.IsAWeekDay(Arg.Any<DateTime>()).Returns(false);
            var service = new SchedulingCalendarService(weekDayGateway);
            var schedulings = new List<Scheduling>();
            var scheduling = new Scheduling(Sunday());

            //act

            //assert
            Assert.Throws<ArgumentException>(() => service.Add(scheduling, schedulings, ReferenceDate()));
        }

        private static DateTime Sunday()
        {
            return new DateTime(2019, 6, 30, 12, 0, 0);
        }

        [Fact]
        public void AddSchedulingOnSaturday_Error()
        {
            //arrange
            var weekDayGateway = Substitute.For<IWeekDayGateway>();
            weekDayGateway.IsAWeekDay(Arg.Any<DateTime>()).Returns(false);
            var service = new SchedulingCalendarService(weekDayGateway);
            var schedulings = new List<Scheduling>();
            var scheduling = new Scheduling(Saturday());

            //act

            //assert
            Assert.Throws<ArgumentException>(() => service.Add(scheduling, schedulings, ReferenceDate()));
        }

        private static DateTime Saturday()
        {
            return new DateTime(2019, 6, 29, 12, 0, 0);
        }

        [Fact]
        public void AddSchedulingOnHoliday_Error()
        {
            //arrange
            var weekDayGateway = Substitute.For<IWeekDayGateway>();
            weekDayGateway.IsAWeekDay(Arg.Any<DateTime>()).Returns(false);
            var service = new SchedulingCalendarService(weekDayGateway);
            var schedulings = new List<Scheduling>();
            var scheduling = new Scheduling(Christmas());

            //act

            //assert
            Assert.Throws<ArgumentException>(() => service.Add(scheduling, schedulings, ReferenceDate()));
        }

        [Fact]
        public void AddSchedulingBeforeSixAm_Error()
        {
            //arrange
            var weekDayGateway = Substitute.For<IWeekDayGateway>();
            weekDayGateway.IsAWeekDay(Arg.Any<DateTime>()).Returns(true);
            var service = new SchedulingCalendarService(weekDayGateway);
            var schedulings = new List<Scheduling>();
            var scheduling = new Scheduling(WeekDay(6).AddMinutes(-1));

            //act

            //assert
            Assert.Throws<ArgumentException>(() => service.Add(scheduling, schedulings, ReferenceDate()));
        }

        [Fact]
        public void AddSchedulingAtSixAm_Success()
        {
            //arrange
            var weekDayGateway = Substitute.For<IWeekDayGateway>();
            weekDayGateway.IsAWeekDay(Arg.Any<DateTime>()).Returns(true);
            var service = new SchedulingCalendarService(weekDayGateway);
            var schedulings = new List<Scheduling>();
            var scheduling = new Scheduling(WeekDay(6));

            //act
            service.Add(scheduling, schedulings, ReferenceDate());

            //assert
            Assert.True(schedulings.Count > 0);
        }

        [Fact]
        public void AddSchedulingAtSixPm_Success()
        {
            //arrange
            var weekDayGateway = Substitute.For<IWeekDayGateway>();
            weekDayGateway.IsAWeekDay(Arg.Any<DateTime>()).Returns(true);
            var service = new SchedulingCalendarService(weekDayGateway);
            var schedulings = new List<Scheduling>();
            var scheduling = new Scheduling(WeekDay(18));

            //act
            service.Add(scheduling, schedulings, ReferenceDate());

            //assert
            Assert.True(schedulings.Count > 0);
        }

        [Fact]
        public void AddSchedulingAfterSixPm_Error()
        {
            //arrange
            var weekDayGateway = Substitute.For<IWeekDayGateway>();
            weekDayGateway.IsAWeekDay(Arg.Any<DateTime>()).Returns(true);
            var service = new SchedulingCalendarService(weekDayGateway);
            var schedulings = new List<Scheduling>();
            var scheduling = new Scheduling(WeekDay(18).AddMinutes(1));

            //act

            //assert
            Assert.Throws<ArgumentException>(() => service.Add(scheduling, schedulings, ReferenceDate()));
        }

        [Fact]
        public void AddRepeteadScheduling_Error()
        {
            //arrange
            var weekDayGateway = Substitute.For<IWeekDayGateway>();
            weekDayGateway.IsAWeekDay(Arg.Any<DateTime>()).Returns(true);
            var service = new SchedulingCalendarService(weekDayGateway);
            var schedulings = new List<Scheduling>();
            var scheduling = new Scheduling(WeekDay(9));
            var schedulingTwo = new Scheduling(WeekDay(9));

            //act
            service.Add(scheduling, schedulings, ReferenceDate());

            //assert
            Assert.Throws<ArgumentException>(() => service.Add(schedulingTwo, schedulings, ReferenceDate()));
        }

        private static DateTime Christmas()
        {
            return new DateTime(2019, 12, 25,12,0,0);
        }

        private static DateTime WeekDay(int hour)
        {
            return new DateTime(2019, 6, 27, hour, 0, 0);
        }

        private static DateTime ReferenceDate()
        {
            return new DateTime(2019, 6, 26, 12, 0, 0);
        }
    }
}
