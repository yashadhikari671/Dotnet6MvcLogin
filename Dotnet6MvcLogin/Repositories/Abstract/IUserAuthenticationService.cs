using Dotnet6MvcLogin.Models.DTO;

namespace Dotnet6MvcLogin.Repositories.Abstract
{
    public interface IUserAuthenticationService
    {
   
        Task<Status> LoginAsync(LoginModel model);
        Task<Status> TwofactorAuth(otpvalidation model);
        Task LogoutAsync();
        Task<Status> RegisterAsync(RegistrationModel model);
        Task<Status> ChangePasswordAsync(ChangePasswordModel model, string username);
    }
}
