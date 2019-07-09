using System;
using LegacySystemsIntegration.Interfaces;
using LegacySystemsIntegration.Models;

namespace LegacySystemsIntegration
{
    public class IntegrationFacade : IIntegrationFacade
    {
        public VerifyDebitsResponse VerifyDebits()
        {
            return new VerifyDebitsResponse();
        }
    }
}
