using System;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.ApplicationCore.Interfaces;

public interface IExperimentationService
{
    Task MapBasketToSessionAsync(int basketId, Guid sessionId);
    Task SaveExperimentAssignmentAsync(Guid sessionId, string experimentName, string variantId);
}
