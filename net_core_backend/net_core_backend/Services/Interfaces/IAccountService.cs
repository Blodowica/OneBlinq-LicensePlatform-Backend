using net_core_backend.Models;
using System.Threading.Tasks;

namespace net_core_backend.Services.Interfaces
{
    public interface IAccountService
    {
        Task<Users> GetUserDetailsJWT(int id);
        Task<VerificationResponse> Login(LoginRequest model, string ipAddress = null);
        Task<VerificationResponse> RefreshToken(string token, string ipaddress);
        Task<VerificationResponse> Register(AddUserRequest requestInfo, string ipAddress = null);
        Task<bool> RevokeToken(string token, string ipAddress);
        Task CreateAdmin(AddUserRequest requestInfo);
        Task ChangePassword(ChangePasswordRequest model);
        Task<EditUserInfoModel> GetUserInfoDetails();
        Task ChangeUserInfoDetails(EditUserInfoModel model);
    }
}