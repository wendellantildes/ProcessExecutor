using System;
using System.Collections.Generic;
using ProcessExecutor.Domain.Processes;
using System.Linq;
using System.Linq.Expressions;
using LegacySystemsIntegration;
using ProcessExecutor.Domain.Processes.Interfaces;
using LegacySystemsIntegration.Interfaces;

namespace ProcessExecutor.Infra.Services.LegacySystemIntegration
{
    public class VerifyDebitsService : IVerifyDebitsService
    {
        private readonly IIntegrationFacade _integrationFacade;

        public VerifyDebitsService(IIntegrationFacade integrationFacade)
        {
            _integrationFacade = integrationFacade;
        }

        public void Verify(Process process, List<string> filesOfTheDay)
        {
            var tasks = process.TasksFromStep(Step.VerifyDebits);
            if(tasks.Count == 0)
            {
                return;
            }
            foreach(var task in tasks)
            {
                process.Start(task);
            }

            var reports = _integrationFacade.VerifyDebits();

            foreach(var task in tasks)
            {
                var foundFiles = reports.ExecutionReports[task.System];
                if(foundFiles == null) {
                    process.MarkWithError(task);
                    continue;
                }

                var newFiles = foundFiles.Except(filesOfTheDay).ToList();

                if(newFiles.Count > 0)
                {
                    task.Add(newFiles.Select(x =>  Variable.DebitFile(task.Id, x)).ToList());
                    process.Finish(task);
                }
                else
                {
                    process.MarkWithError(task);
                }
            }
        }
    }
}
