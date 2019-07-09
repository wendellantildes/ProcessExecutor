using System;
namespace ProcessExecutor.Domain.Processes
{
    public class Variable
    {
        public const string DebitFileName = "debit_file";
        protected Variable(Guid taskId, string name, string value)
        {
            TaskId = taskId;
            Name = name;
            Value = value;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }

        public Guid? TaskId { get; private set; }

        public Guid? ProcessId { get; private set; }

        public string Name { get; private set; }

        public string Value { get; private set; }


        public static Variable DebitFile(Guid taskId, string value)
        {
            return new Variable(taskId, DebitFileName, value);
        }
    }
}
