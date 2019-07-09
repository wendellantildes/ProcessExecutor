using System;
using System.Collections.Generic;
using ProcessExecutor.Common.Domain;

namespace ProcessExecutor.Domain.Processes
{
    public class Task
    {
        public Task(Guid paymentId, LegacySystem system, Step step)
        {
            PaymentId = paymentId;
            System = system;
            Step = step;
            Variables = new List<Variable>();
            Status = Status.Created;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }

        public Guid PaymentId { get; private set; }

        public LegacySystem System { get; set; }

        public Step Step { get; set; }

        public List<Variable> Variables { get; private set; }

        public Status Status { get; private set; }

        public void Add(Variable variable)
        {
            this.Variables.Add(variable);
        }

        public void Add(List<Variable> variables)
        {
            this.Variables.AddRange(variables);
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

        public void Finish()
        {
            if(Status == Status.Started)
            {
                Status = Status.Finished;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void MarkWithError()
        {
            if (Status == Status.Started)
            {
                Status = Status.WithError;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
