using net_core_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Services.Interfaces
{
    public interface IUserService
    {
        Task<GetUserResponse> GetUserDetails(int userId);
        Task EditUser(EditUserRequest request, int userId);
    }
}
