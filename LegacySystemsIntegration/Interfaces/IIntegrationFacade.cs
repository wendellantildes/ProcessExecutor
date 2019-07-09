using System;
using LegacySystemsIntegration.Models;

namespace LegacySystemsIntegration.Interfaces
{
    public interface IIntegrationFacade
    {
        VerifyDebitsResponse VerifyDebits();
    }
}
