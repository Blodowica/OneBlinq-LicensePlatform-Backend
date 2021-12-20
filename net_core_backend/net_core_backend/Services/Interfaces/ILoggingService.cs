using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Services.Interfaces
{
    public interface ILoggingService
    {
        Task AddActivationLog(string licenseKey, bool successful, string externalUniqueUserId, string platformName, string message);
        Task RemoveUniqueUserIdLogs(int id, int licenseId);
    }
}
