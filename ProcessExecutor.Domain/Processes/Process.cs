using System;
using System.Collections.Generic;
using System.Linq;
using ProcessExecutor.Common.Domain;

namespace ProcessExecutor.Domain.Processes
{
    public class Process
    {
        public Process(List<Task> tasks) : this (Guid.NewGuid(), tasks)
        {

        }

        protected Process(Guid id, List<Task> tasks)
        {
            Tasks = tasks;
            Id = id;
            Status = Status.Created;
        }

        public Guid Id { get; private set; }

        public List<Task> Tasks { get; private set; }

        public Status Status { get; private set; }


        public IReadOnlyList<Task> TasksFromStep(Step step)
        {
            return Tasks.Where(x => x.Step == step).ToList().AsReadOnly();
        }

        public void Start()
        {
            if (Status == Status.Created)
            {
                Status = Status.Started;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }


        public void Start(Task task)
        {
            if (Owns(task) && Status == Status.Started)
            {
                task.Start();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void MarkWithError(Task task)
        {
            if (Owns(task) && (Status == Status.Started || Status == Status.WithError))
            {
                Status = Status.WithError;
                task.MarkWithError();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void Finish(Task task)
        {
            if (Owns(task) && (Status == Status.Started || Status == Status.WithError))
            {
                task.Finish();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private bool Owns(Task task)
        {
            return Tasks.Any(x => x.Id == task.Id);
        }


        public static Process NewDefaultSalaryPaymentProcess()
        {
            var id = Guid.NewGuid();
            var taskLoadCredits = new Task(id, LegacySystem.CreditSource, Step.LoadCredits);
            var taskVerifyCredits = new Task(id, LegacySystem.CreditSource, Step.VerifyCredits);
            var taskLoadDebitsOnlineSource = new Task(id, LegacySystem.OnlineDebitSource, Step.LoadDebits);
            var taskLoadDebitsOfflineSource = new Task(id, LegacySystem.OfflineDebitSource, Step.LoadCredits);
            var taskVerifyDebitsOnlineSource = new Task(id, LegacySystem.OnlineDebitSource, Step.VerifyDebits);
            var taskVerifyDebitsOfflineSource = new Task(id, LegacySystem.OfflineDebitSource, Step.VerifyDebits);
            var taskPaySalary = new Task(id, LegacySystem.Payer, Step.PaySalary);
            var tasks = new List<Task>
            {
                taskLoadCredits,
                taskVerifyCredits,
                taskLoadDebitsOnlineSource,
                taskLoadDebitsOfflineSource,
                taskVerifyDebitsOnlineSource,
                taskVerifyDebitsOfflineSource,
                taskPaySalary
            };
            return new Process(id, tasks);
        }
    }
}
