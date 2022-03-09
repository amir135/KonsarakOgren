using KonOgren.Domain.Dto;
using KonOgren.Domain.Model;
using KonOgren.Infrastructure.Result;
using KonOgren.Infrastructure.ViewModel;
using System;

namespace KonOgren.Services
{
    public interface IUserService
    {
        Result<User> GetUserByEmail(string Emailaddress);
        Result<User> Login(LoginViewModel model);
        Result<User> GetUser(Int64 userId);
        Result<UserDto> GetActiveUser();
    }
}
