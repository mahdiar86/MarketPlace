using MarketPlace.Data.DTOs.Account;
using MarketPlace.Data.Entities.Account;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MarketPlace.Application.Services.Interfaces
{
    public interface IUserService : IAsyncDisposable
    {
        Task<RegisterUserResult> RegisterUser(RegisterUserDTO register);
        Task<bool> IsUserExsitsByMobileNumber(string mobile);
        Task<LoginUserResult> GetUserForLogin(LoginUserDTO login);
        Task<User> GetUserByMobile(string mobile);
        Task<ForgotPasswordResult> RecoverUserPassword(ForgotPasswordDTO forgot);
        Task<bool> ActivateMobile(ActivateMobileDTO activateMobileDTO);
        Task<ChangePasswordResult> ChangeUserPassword(ChangePasswordDTO passwordDto,string mobileNumber);
        Task<EditUserProfileDTO> GetProfileForEdit(long userId);
        Task<EditUserProfileResult> EditUserProfile(EditUserProfileDTO profile,long userId,IFormFile avatarImage);
        Task<User> GetUserById(long userId);
    }
}
