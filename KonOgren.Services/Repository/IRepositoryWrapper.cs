using System;
using System.Collections.Generic;
using System.Text;

namespace KonOgren.Services.Repository
{
    public interface IRepositoryWrapper
    {
  
        IUserRepository User { get; }
        IExamRepository Exam { get; }
        IQuestionRepository Question { get; }


        void Save();
    }

}
