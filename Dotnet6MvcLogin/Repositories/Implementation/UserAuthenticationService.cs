using Dotnet6MvcLogin.Models;
using Dotnet6MvcLogin.Models.Domain;
using Dotnet6MvcLogin.Models.DTO;
using Dotnet6MvcLogin.Models.Mail;
using Dotnet6MvcLogin.Repositories.Abstract;
using Dotnet6MvcLogin.Services.MailingService;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;

namespace Dotnet6MvcLogin.Repositories.Implementation
{
    public class UserAuthenticationService: IUserAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMailingService _mailingService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public UserAuthenticationService(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IMailingService mailingService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mailingService = mailingService;
            _signInManager = signInManager; 

        }

        public async Task<Status> RegisterAsync(RegistrationModel model)
        {
            var status = new Status();
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                status.StatusCode = 0;
                status.Message = "User already exist";
                return status;
            }
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                FirstName = model.FirstName,
                LastName=model.LastName,
                EmailConfirmed=true,
                PhoneNumberConfirmed=true,
                TwoFactorEnabled=true,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "User creation failed";
                return status;
            }

            if (!await _roleManager.RoleExistsAsync(model.Role))
                await _roleManager.CreateAsync(new IdentityRole(model.Role));
            

            if (await _roleManager.RoleExistsAsync(model.Role))
            {
                await _userManager.AddToRoleAsync(user, model.Role);
            }

            status.StatusCode = 1;
            status.Message = "You have registered successfully";
            return status;
        }


        public async Task<Status> LoginAsync(LoginModel model)
        {
            var status = new Status();
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                status.StatusCode = 0;
                status.Message = "Invalid username";
                return status;
            }

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                status.StatusCode = 0;
                status.Message = "Invalid Password";
                return status;
            }
            if (user.TwoFactorEnabled)
            {
                var token = await _userManager.GenerateTwoFactorTokenAsync(user,  TokenOptions.DefaultPhoneProvider);
                var mailRequest = new MailRequestModel
                {
                    MailFor = "invoice",
                    MailTo = "yash.adhikari671@gmail.com",
                    MailSubject = $"otp is : {token} ",
                    RecipientName = "yash Adhikari",
                    Content = $"otp is : {token} "

                };
                var mailServiceModel = await _mailingService.EmailSettings(mailRequest);

                Thread email = new(delegate ()
                {
                    _mailingService.SendMail(mailServiceModel);
                });
                email.Start();
                //var sendmail = await _mailingService.SendMail(mailServiceModel);

                status.StatusCode = 2;
                status.Message = "Mail has been send successfully";
                return status;

            }

            var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, true);
            if (signInResult.Succeeded)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                    status.StatusCode = 1;
                    status.Message = "Logged in successfully";
          

            }
            else if (signInResult.IsLockedOut)
            {
                status.StatusCode = 0;
                status.Message = "User is locked out";
            }
            else
            {
                status.StatusCode = 0;
                status.Message = "Error on logging in";
            }
           
            return status;
        }  
        public async Task<Status> TwofactorAuth(otpvalidation model)
        {
            var status = new Status();
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                status.StatusCode = 0;
                status.Message = "Invalid username";
                return status;
            }

            var signInResult = await _signInManager.TwoFactorAuthenticatorSignInAsync(model.otp,false,false);
            if (signInResult.Succeeded)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                    status.StatusCode = 1;
                    status.Message = "Logged in successfully";
          

            }
            else if (signInResult.IsLockedOut)
            {
                status.StatusCode = 0;
                status.Message = "User is locked out";
            }
            else
            {
                status.StatusCode = 0;
                status.Message = "Error on logging in";
            }
           
            return status;
        }

        public async Task LogoutAsync()
        {
           await _signInManager.SignOutAsync();
           
        }

        public async Task<Status> ChangePasswordAsync(ChangePasswordModel model,string username)
        {
            var status = new Status();
            
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                status.Message = "User does not exist";
                status.StatusCode = 0;
                return status;
            }
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                status.Message = "Password has updated successfully";
                status.StatusCode = 1;
            }
            else
            {
                status.Message = "Some error occcured";
                status.StatusCode = 0;
            }
            return status;

        }
    }
}
