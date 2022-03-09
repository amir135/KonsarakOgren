using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace KonOgren.Infrastructure.Helper
{
    public static class UserHelper
    {
        public static string GetUserId(IHttpContextAccessor contextAccessor)
        {
            string userId = "";
            if (null != contextAccessor)
            {
                if (contextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated == true)
                {
                    userId = contextAccessor.HttpContext.User.FindFirst("UserId")?.Value;
                }
                else
                {
                    userId = contextAccessor?.HttpContext?.Request?.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                }
            }
            return userId;
        }
    }
}
