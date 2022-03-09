using KonOgren.DataAccess;
using KonOgren.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace KonOgren.Services.Repository
{
    public class ExamRepository : Repository<Exam>, IExamRepository
    {
        public ExamRepository(KonOgrenDBContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
