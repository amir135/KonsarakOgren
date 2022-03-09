using System;
using System.Collections.Generic;
using System.Text;

namespace KonOgren.Infrastructure.StaticContents
{
    public class StaticConfiguration
    {
        public static string ConnectionString { get; set; }
      
        public static string JWTSecretKey { get; set; }
        public static string wwwrootPath { get; set; } 
       
        public static string ContentRootPath { get; set; }
    }
}
