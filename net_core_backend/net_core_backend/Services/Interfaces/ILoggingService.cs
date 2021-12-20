using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Services.Interfaces
{
    public interface ILoggingService
    {
        Task AddActivationLog(String licenseKey, bool successful, String figmaUserId, string platformName, string message);
        Task RemoveUniqueUserIdLogs(int id, int licenseId);
    }
}
