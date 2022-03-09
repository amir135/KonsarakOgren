
using AutoMapper;
using EFCore.BulkExtensions;
using KonOgren.DataAccess;
using KonOgren.Domain.Model;
using KonOgren.Infrastructure.StaticContents;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KonOgren.DataAccess
{

    public class SeedDB
    {
        IMapper _mapper;
        public SeedDB(IMapper mapper)
        {
            _mapper = mapper;

        }
        public static void Initialize(KonOgrenDBContext context)
        {
            var now = DateTime.Now;
            string projectRootPath = StaticConfiguration.wwwrootPath;

            List<User> user = new List<User>()
                {
                   new User()
                   {
                       Email = "admin@gmail.com",
                       Name="Admin",
                       Surname="Admin System", // TODO Implement Hash Password
                       Password=BCrypt.Net.BCrypt.HashPassword( "123456"),
                   }
                };

            if (!context.Users.Any())
            {
                context.AddRange(user);
                context.SaveChanges();
            }

         }
    }
}
