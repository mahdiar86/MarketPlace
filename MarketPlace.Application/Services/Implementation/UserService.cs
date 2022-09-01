using MarketPlace.Application.Services.Interfaces;
using MarketPlace.Data.DTOs.Account;
using MarketPlace.Data.Entities.Account;
using MarketPlace.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MarketPlace.Application.Extensions;
using MarketPlace.Application.Utils;
using Microsoft.AspNetCore.Http;

namespace MarketPlace.Application.Services.Implementation
{
    public class UserService : IUserService
    {
        #region constructor 

        private readonly IGenericRepository<User> _userRepository;
        private readonly IPasswordHelper _passwordHelper;
        private readonly ISmsService _smsService;

        public UserService(IGenericRepository<User> userRepository, IPasswordHelper passwordHelper, ISmsService smsService)
        {
            _userRepository = userRepository;
            _passwordHelper = passwordHelper;
            _smsService = smsService;
        }

        #endregion

        #region Dispose 

        public async ValueTask DisposeAsync()
        {
            await _userRepository.DisposeAsync();
        }

        #endregion

        public async Task<User> GetUserByMobile(string mobile)
        {
            return await _userRepository.GetQuery().AsQueryable()
                .SingleOrDefaultAsync(m => m.Mobile == mobile);
        }

        public async Task<LoginUserResult> GetUserForLogin(LoginUserDTO login)
        {
            var user = await _userRepository.GetQuery().AsQueryable()
                .SingleOrDefaultAsync(u => u.Mobile == login.Mobile);

            if (user == null)
                return LoginUserResult.NotFound;
            if (!user.IsMobileActive)
                return LoginUserResult.NotActivated;
            if (user.Password != _passwordHelper.EncodePasswordMd5(login.Password))
                return LoginUserResult.NotFound;

            return LoginUserResult.Success;

        }

        public async Task<bool> IsUserExsitsByMobileNumber(string mobile)
        {
            return await _userRepository.GetQuery().AsQueryable().AnyAsync(s => s.Mobile == mobile);
        }

        public async Task<ForgotPasswordResult> RecoverUserPassword(ForgotPasswordDTO forgot)
        {
            var user = await _userRepository.GetQuery().SingleOrDefaultAsync(u => u.Mobile == forgot.Mobile);

            if (user == null)
                return ForgotPasswordResult.NotFound;

            if (user.IsBlocked)
                return ForgotPasswordResult.Error;

            var password = new Random().Next(1000000, 9999999).ToString();
            user.Password = _passwordHelper.EncodePasswordMd5(password);

            _userRepository.EditEntity(user);

            await _smsService.SendRecoveryPasswordSms(password, user.Mobile);

            await _userRepository.SaveChanges();

            return ForgotPasswordResult.Success;
        }

        public async Task<RegisterUserResult> RegisterUser(RegisterUserDTO register)
        {
            if (!await IsUserExsitsByMobileNumber(register.Mobile))
            {
                var user = new User()
                {
                    Mobile = register.Mobile,
                    Password = _passwordHelper.EncodePasswordMd5(register.Password),
                    MobileActiveCode = new Random().Next(10000, 99999).ToString(),
                    EmailActiveCode = Guid.NewGuid().ToString("N")
                };

                await _userRepository.AddEntity(user);
                await _userRepository.SaveChanges();
                await _smsService.SendVerificationSms(user.MobileActiveCode, user.Mobile);


                return RegisterUserResult.Success;

            }

            return RegisterUserResult.MobileExists;

        }

        public async Task<bool> ActivateMobile(ActivateMobileDTO activateMobileDTO)
        {
            var user = await _userRepository.GetQuery().AsQueryable()
                .SingleOrDefaultAsync(s => s.Mobile == activateMobileDTO.Mobile);
            if (user != null)
            {
                if (user.MobileActiveCode == activateMobileDTO.MobileActiveCode)
                {
                    user.IsMobileActive = true;
                    user.MobileActiveCode = new Random().Next(100000, 1000000).ToString();
                    await _userRepository.SaveChanges();
                    return true;
                }
            }

            return false;
        }

        public async Task<ChangePasswordResult> ChangeUserPassword(ChangePasswordDTO passwordDto, string mobileNumber)
        {
            var user = await GetUserByMobile(mobileNumber);

            if (user == null)
                return ChangePasswordResult.Error;

            var password = _passwordHelper.EncodePasswordMd5(passwordDto.CurrentPassword);
            if (user.Password == password)
            {
                user.Password = _passwordHelper.EncodePasswordMd5(passwordDto.NewPassword);
                _userRepository.EditEntity(user);
                await _userRepository.SaveChanges();
                return ChangePasswordResult.Success;
            }

            return ChangePasswordResult.PasswordInValid;
        }

        public async Task<EditUserProfileDTO> GetProfileForEdit(long userId)
        {
            var user = await _userRepository.GetEntityById(userId);

            if (user == null)
                return null;

            return new EditUserProfileDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Avatar = user.Avatar
            };
        }

        public async Task<EditUserProfileResult> EditUserProfile(EditUserProfileDTO profile, long userId, IFormFile avatarImage)
        {
            var user = await _userRepository.GetEntityById(userId);
            if (user == null)
                return EditUserProfileResult.NotFound;
            if (user.IsBlocked)
                return EditUserProfileResult.IsBlocked;
            if (!user.IsMobileActive)
                return EditUserProfileResult.IsNotActive;

            user.FirstName = profile.FirstName;
            user.LastName = profile.LastName;

            if (avatarImage != null && avatarImage.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N")+Path.GetExtension(avatarImage.FileName);
                avatarImage.AddImageToServer(imageName,FilePath.UserAvatarServerOrigin,120,120,FilePath.UserAvatarServerThumb,user.Avatar);
                user.Avatar = imageName;
            }

            _userRepository.EditEntity(user);
            await _userRepository.SaveChanges();

            return EditUserProfileResult.Success;
        }

        public Task<User> GetUserById(long userId)
        {
            return _userRepository.GetEntityById(userId);
        }
    }
}
