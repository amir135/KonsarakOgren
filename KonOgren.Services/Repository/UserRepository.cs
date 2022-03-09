using KonOgren.DataAccess;
using KonOgren.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace KonOgren.Services.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(KonOgrenDBContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
