using AutoMapper;
using KonOgren.DataAccess;
using KonOgren.Domain.Dto;
using KonOgren.Domain.Model;
using KonOgren.Infrastructure.Helper;
using KonOgren.Infrastructure.Result;
using KonOgren.Infrastructure.ViewModel;
using KonOgren.Services.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KonOgren.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;
        private readonly KonOgrenDBContext _context;
        public UserService(IHttpContextAccessor contextAccessor, IMapper mapper, IRepositoryWrapper repository, KonOgrenDBContext context)
        {
            _repository = repository;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
            _context = context;
        }

        public Result<User> GetUser(Int64 userId)
        {
            string userid = UserHelper.GetUserId(_contextAccessor);
            Result<User> result = new Result<User>();
            try
            {
                var user = _repository.User.GetById(userId);
                if (user != null)
                {
                    result.Data = user;
                    result.Success = true;
                }
                else
                {

                    result.Success = false;
                    result.Message = "Requested User is not Exists!";
                }

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Internal error. Please try again later.";
            }
            return result;
        }


        public Result<User> Login(LoginViewModel model)
        {
            string userid = UserHelper.GetUserId(_contextAccessor);


            Result<User> result = new Result<User>();
            try
            {
                var users = _repository.User.Get().Where( a => a.Email == model.UserName && !a.IsDeleted && a.IsActive).ToList();
                if (users.Any())
                {
                    var user = users.FirstOrDefault(a => BCrypt.Net.BCrypt.Verify(model.Password, a.Password));
                    if (user != null)
                    {
                        result.Data = user;
                        result.Success = true;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = "Username or Password is not correct!";
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = "Username or Password is not correct!";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Internal error. Please try again later.";
            }
            return result;
        }

        public Result<User> GetUserByEmail(string Emailaddress)
        {
            string userid = UserHelper.GetUserId(_contextAccessor);
            Result<User> result = new Result<User>();
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == Emailaddress);
                if (user != null)
                {
                    result.Data = user;
                    result.Success = true;
                }
                else
                {

                    result.Success = false;
                    result.Message = "Requested User is not Exists!";
                }

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Internal error. Please try again later.";
            }
            return result;
        }

        public Result<UserDto> GetActiveUser()
        {
            Result<UserDto> result = new Result<UserDto>();
            try
            {
                var userId = Int64.Parse(_contextAccessor.HttpContext.User.FindFirst("UserId").Value);
                var user = _repository.User.GetById(userId);
                if (user != null)
                {
                    result.Data = _mapper.Map<UserDto>(user);
                    result.Success = true;
                }
                else {
                    result.Success = false;
                }
            }
            catch (Exception)
            {
                result.Success = false;
            }
            return result;
        }
    }
}
