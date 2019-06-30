using System;
namespace ProcessExecutor.Domain
{
    public class Scheduling
    {
        public Scheduling(DateTime date)
        {
            Id = Guid.NewGuid();
            Date = date;
            Finished = false;
        }

        public Guid Id { get; private set; }

        public DateTime Date { get; private set; }

        public bool Finished { get; private set; }

        public void Finish()
        {
            Finished = true;
        }
    }
}
