using System;
using System.Collections.Generic;
using ProcessExecutor.Common.Domain;

namespace LegacySystemsIntegration.Models
{
    public class VerifyDebitsResponse
    {
        public Dictionary<LegacySystem,List<string>> ExecutionReports{ get; set;}
    }
}
