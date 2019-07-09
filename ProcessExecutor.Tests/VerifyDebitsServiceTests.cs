using System;
using System.Collections.Generic;
using LegacySystemsIntegration.Interfaces;
using LegacySystemsIntegration.Models;
using NSubstitute;
using ProcessExecutor.Common.Domain;
using ProcessExecutor.Domain.Processes;
using ProcessExecutor.Infra.Services.LegacySystemIntegration;
using Xunit;

namespace ProcessExecutor.Tests
{
    public class VerifyDebitsServiceTests
    {
        private const string OfflineAFile = "offline_debit_a";
        private const string OfflineBFile = "offline_debit_b";
        private const string OfflineCFile = "offline_debit_c";
        private const string OfflineDFile = "offline_debit_d";
        private const string OnlineAFile = "online_debit_a";
        private const string OnlineBFile = "online_debit_b";
        private const string OnlineCFile = "online_debit_c";
        private const string OnlineDFile = "online_debit_d";

        private Process GetNew()
        {
            var process = Process.NewDefaultSalaryPaymentProcess();
            process.Start();
            foreach(var step in Enum.GetValues(typeof(Step)))
            {
                if((Step)step == Step.VerifyDebits)
                {
                    break;
                }
                var tasks = process.TasksFromStep((Step)step);
                foreach(var task in tasks)
                {
                    process.Start(task);
                    process.Finish(task);
                }
            }
            return process;
        }

        [Fact]
        public void FirstExecutionOfTheDay_FindNewFiles_FinishTask()
        {
            var files = new List<string>();
            var integrationFacade = Substitute.For<IIntegrationFacade>();
            var reports = new Dictionary<LegacySystem, List<string>>();
            reports.Add(LegacySystem.OfflineDebitSource, new List<string>()
            {
                OfflineAFile
            });
            reports.Add(LegacySystem.OnlineDebitSource, new List<string>()
            {
                OnlineAFile
            });
            integrationFacade.VerifyDebits().Returns(new VerifyDebitsResponse()
            {
                ExecutionReports = reports
            });
            var service = new VerifyDebitsService(integrationFacade);
            var process = GetNew();

            service.Verify(process, files);

            Assert.Equal(Status.Started, process.Status);
            var tasks = process.TasksFromStep(Step.VerifyDebits);
            Assert.All(tasks, (task) =>
            {
                Assert.Equal(Status.Finished, task.Status);
                Assert.True(task.Variables.Count == 1);
            });
        }

        [Fact]
        public void SecondExecutionOfTheDay_FindNewFiles_FinishTask()
        {
            var files = new List<string>()
            {
                OfflineAFile, OnlineAFile
            };
            var integrationFacade = Substitute.For<IIntegrationFacade>();
            var reports = new Dictionary<LegacySystem, List<string>>();
            reports.Add(LegacySystem.OfflineDebitSource, new List<string>()
            {
                OfflineAFile, OfflineBFile
            });
            reports.Add(LegacySystem.OnlineDebitSource, new List<string>()
            {
                OnlineAFile, OnlineBFile
            });
            integrationFacade.VerifyDebits().Returns(new VerifyDebitsResponse()
            {
                ExecutionReports = reports
            });
            var service = new VerifyDebitsService(integrationFacade);
            var process = GetNew();

            service.Verify(process, files);

            Assert.Equal(Status.Started, process.Status);
            var tasks = process.TasksFromStep(Step.VerifyDebits);
            Assert.All(tasks, (task) =>
            {
                Assert.Equal(Status.Finished, task.Status);
                Assert.True(task.Variables.Count == 1);
            });
        }

        [Fact]
        public void SecondExecutionOfTheDay_DoNotFindNewFiles_MarkTask_AndProcess_WithError()
        {
            var files = new List<string>()
            {
                OfflineAFile, OnlineAFile
            };
            var integrationFacade = Substitute.For<IIntegrationFacade>();
            var reports = new Dictionary<LegacySystem, List<string>>();
            reports.Add(LegacySystem.OfflineDebitSource, new List<string>());
            reports.Add(LegacySystem.OnlineDebitSource, new List<string>());
            integrationFacade.VerifyDebits().Returns(new VerifyDebitsResponse()
            {
                ExecutionReports = reports
            });
            var service = new VerifyDebitsService(integrationFacade);
            var process = GetNew();

            service.Verify(process, files);

            Assert.Equal(Status.WithError, process.Status);
            var tasks = process.TasksFromStep(Step.VerifyDebits);
            Assert.All(tasks, (task) =>
            {
                Assert.Equal(Status.WithError, task.Status);
                Assert.True(task.Variables.Count == 0);
            });
        }

        [Fact]
        public void SecondExecutionOfTheDay_FindNewFilesJustForOnlineSource_MarkTaskJustOnlineTaskAsFinished_AndMark_OfflineTask_And_Process_WithError()
        {
            var files = new List<string>()
            {
                OfflineAFile, OnlineAFile
            };
            var integrationFacade = Substitute.For<IIntegrationFacade>();
            var reports = new Dictionary<LegacySystem, List<string>>();
            reports.Add(LegacySystem.OfflineDebitSource, new List<string>());
            reports.Add(LegacySystem.OnlineDebitSource, new List<string>()
            {
                OnlineAFile, OnlineBFile
            });
            integrationFacade.VerifyDebits().Returns(new VerifyDebitsResponse()
            {
                ExecutionReports = reports
            });
            var service = new VerifyDebitsService(integrationFacade);
            var process = GetNew();

            service.Verify(process, files);

            Assert.Equal(Status.WithError, process.Status);
            var tasks = process.TasksFromStep(Step.VerifyDebits);
            Assert.All(tasks, (task) =>
            {
                if(task.System == LegacySystem.OnlineDebitSource)
                {
                    Assert.Equal(Status.Finished, task.Status);
                    Assert.True(task.Variables.Count == 1);
                }
                else
                {
                    Assert.Equal(Status.WithError, task.Status);
                    Assert.True(task.Variables.Count == 0);
                }
            });
        }

        [Fact]
        public void SecondExecutionOfTheDay_FindNewFilesJustForOfflineSource_MarkTaskJustOfflineTaskAsFinished_AndMark_OnlineTask_And_Process_WithError()
        {
            var files = new List<string>()
            {
                OfflineAFile, OnlineAFile
            };
            var integrationFacade = Substitute.For<IIntegrationFacade>();
            var reports = new Dictionary<LegacySystem, List<string>>();
            reports.Add(LegacySystem.OfflineDebitSource, new List<string>()
            {
                OfflineAFile, OfflineBFile
            });
            reports.Add(LegacySystem.OnlineDebitSource, new List<string>());
            integrationFacade.VerifyDebits().Returns(new VerifyDebitsResponse()
            {
                ExecutionReports = reports
            });
            var service = new VerifyDebitsService(integrationFacade);
            var process = GetNew();

            service.Verify(process, files);

            Assert.Equal(Status.WithError, process.Status);
            var tasks = process.TasksFromStep(Step.VerifyDebits);
            Assert.All(tasks, (task) =>
            {
                if (task.System == LegacySystem.OfflineDebitSource)
                {
                    Assert.Equal(Status.Finished, task.Status);
                    Assert.True(task.Variables.Count == 1);
                }
                else
                {
                    Assert.Equal(Status.WithError, task.Status);
                    Assert.True(task.Variables.Count == 0);
                }
            });
        }

        [Fact]
        public void SecondExecutionOfTheDay_FindMoreThanOneNewFile_FinishTask()
        {
            var files = new List<string>()
            {
                OfflineAFile, OnlineAFile
            };
            var integrationFacade = Substitute.For<IIntegrationFacade>();
            var reports = new Dictionary<LegacySystem, List<string>>();
            reports.Add(LegacySystem.OfflineDebitSource, new List<string>()
            {
                OfflineAFile, OfflineBFile, OfflineCFile, OfflineDFile
            });
            reports.Add(LegacySystem.OnlineDebitSource, new List<string>()
            {
                OnlineAFile, OnlineBFile, OnlineCFile, OnlineDFile
            });
            integrationFacade.VerifyDebits().Returns(new VerifyDebitsResponse()
            {
                ExecutionReports = reports
            });
            var service = new VerifyDebitsService(integrationFacade);
            var process = GetNew();

            service.Verify(process, files);

            Assert.Equal(Status.Started, process.Status);
            var tasks = process.TasksFromStep(Step.VerifyDebits);
            Assert.All(tasks, (task) =>
            {
                Assert.Equal(Status.Finished, task.Status);
                Assert.True(task.Variables.Count == 3);
            });
        }
    }
}
