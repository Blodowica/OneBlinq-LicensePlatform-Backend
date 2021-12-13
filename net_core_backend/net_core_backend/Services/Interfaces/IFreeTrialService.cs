using net_core_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Services.Interfaces
{
    public interface IFreeTrialService
    {
        Task CreateFreeTrial(FreeTrialRequest model);
        Task<FreeTrialStates> CheckFreeTrial(String pluginName, String figmaUserId);
        Task VerifyFreeTrial(FreeTrialRequest model);
        Task SetEndDate(int freeTrialId, DateTime newEndDate);
        Task ToggleFreeTrial(int freeTrialId);
    }
}
