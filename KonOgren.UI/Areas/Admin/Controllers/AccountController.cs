using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using KonOgren.Infrastructure.ViewModel;
using KonOgren.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

using Microsoft.AspNetCore.Mvc;



namespace KonOgren.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize("admin")]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;


        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// Load Active User Profile
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> Login()
        {

            try
            {
                if (TempData["Message"] != null)
                {
                    ViewBag.Issuccess = (bool)TempData["IsSuccess"];
                    ViewBag.Message = TempData["Message"].ToString();
                }
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            }
            catch (Exception ex)
            {
                // todo log
            }
            return View();

        }

        //[AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Authentication(LoginViewModel model)
        {
            try
            {
                var result = _userService.Login(model);
                if (result.Success)
                {
                    var claims = new List<Claim>
                    {
                        new Claim("UserId", result.Data.Id.ToString()),
                        new Claim("FullName", result.Data.Name+" "+result.Data.Surname),
                        new Claim("Username", result.Data.Email),
                       // new Claim(ClaimTypes.Role, result.Data.Role.Name),
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);


                    var expire = DateTimeOffset.UtcNow.AddDays(1);
                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        IsPersistent = false,
                        ExpiresUtc = expire,
                    };
                    
                    if (model.RememberMe == "on")
                    {
                        authProperties.IsPersistent = true;
                        authProperties.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30);
                       // expire = DateTimeOffset.UtcNow.AddDays(30);
                    }

                 
                   
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["IsSuccess"] = false;
                    TempData["Message"] = result.Message;
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    

    }
}
