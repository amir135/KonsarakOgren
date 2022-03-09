using KonOgren.DataAccess;
using KonOgren.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace KonOgren.Services.Repository
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        public QuestionRepository(KonOgrenDBContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
