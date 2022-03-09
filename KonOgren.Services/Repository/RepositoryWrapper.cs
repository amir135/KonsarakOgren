using KonOgren.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace KonOgren.Services.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private KonOgrenDBContext _repoContext;
        private IUserRepository _user;
        private IExamRepository _exam;
        private IQuestionRepository _question;
        public RepositoryWrapper(KonOgrenDBContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }


        public IUserRepository User
        {
            get
            {
                if (null == _user)
                    _user = new UserRepository(_repoContext);

                return _user;
            }
        }

        public IQuestionRepository Question
        {
            get
            {
                if (null == _question)
                    _question = new QuestionRepository(_repoContext);

                return _question;
            }
        }

        public IExamRepository Exam
        {
            get
            {
                if (null == _exam)
                    _exam = new ExamRepository(_repoContext);

                return _exam;
            }
        }

        public void Save()
        {
            _repoContext.SaveChanges();
        }
    }
}
