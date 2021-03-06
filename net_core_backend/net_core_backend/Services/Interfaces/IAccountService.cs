using net_core_backend.Models;
using System.Threading.Tasks;

namespace net_core_backend.Services.Interfaces
{
    public interface IAccountService
    {
        Task<VerificationResponse> Login(LoginRequest model, string ipAddress = null);
        Task<VerificationResponse> RefreshToken(string token, string ipaddress);
        Task<VerificationResponse> Register(AddUserRequest requestInfo, string ipAddress = null);
        Task CreateAdmin(AddUserRequest requestInfo);
        Task ChangePassword(ChangePasswordRequest model);
        Task<EditUserInfoModel> GetUserInfoDetails();
        Task ChangeUserInfoDetails(EditUserInfoModel model);
        Task<bool> RevokeCookie(string token, string ipAddress);
        Task ForgottenPasswordRequest(string email);
        Task<VerificationResponse> ForgottenPasswordVerification(ForgottenPasswordVerificationRequest request);
        Task<UserNotificationsResponse> GetUserNotifications();
        Task SetUserNotifications(UserNotificationsRequest model);
    }
}