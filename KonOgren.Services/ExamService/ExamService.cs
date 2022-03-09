using AutoMapper;
using KonOgren.DataAccess;
using KonOgren.Domain.Dto;
using KonOgren.Domain.Model;
using KonOgren.Infrastructure.Helper;
using KonOgren.Infrastructure.Result;
using KonOgren.Infrastructure.ViewModel;
using KonOgren.Services.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KonOgren.Services
{
    public class ExamService : IExamService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;
        private readonly KonOgrenDBContext _context;
        public ExamService(IHttpContextAccessor contextAccessor, IMapper mapper, IRepositoryWrapper repository, KonOgrenDBContext context)
        {
            _repository = repository;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
            _context = context;
        }

        public Result<Exam> AddExam(Exam model)
        {
            Result<Exam> exam = new Result<Exam>();
            try
            {
                exam.Data =  _repository.Exam.Add(model);
                exam.Success = true;
            }
            catch (Exception ex)
            {

                exam.Success = false;
            }
            return exam;
        }

        public Result<Question> AddQuestion(Question model)
        {
            Result<Question> result = new Result<Question>();
            try
            {
                result.Data = _repository.Question.Add(model);
                result.Success = true;
            }
            catch (Exception)
            {

                result.Success = false;
            }
            return result;
        }

        public Result DeleteExam(long userId)
        {
            Result result = new Result();
            try
            {
                var exam = _repository.Exam.GetById(userId);
                if (exam != null)
                {
                    exam.IsDeleted = true;
                    _repository.Exam.Update(exam);
                    result.Success = true;
                }
                else
                {
                    result.Success = false;
                    result.Message = "Exam is not exists!";
                }
            }
            catch (Exception)
            {

                result.Success = false;
            }
            return result;
        }

        public Result<List<Exam>> GetAllExams()
        {
            Result<List<Exam>> result = new Result<List<Exam>>();
            try
            {
                var includes = new List<Expression<Func<Exam, object>>>
                {
                    e => e.Questions
                };
                result.Data = _repository.Exam.Get(includes: includes, predicate: x => !x.IsDeleted).ToList();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
            }
            return result;
        }

        public Result<JsonResult> GetAllExamsPagination(DataTableViewModel dataTable)
        {
            Result<JsonResult> result = new Result<JsonResult>();
            try
            {
                int recordsTotal = 0;

                var exams = _repository.Exam.Get(predicate: a => !a.IsDeleted);

                //Sorting
                if (!string.IsNullOrEmpty(dataTable.sortColumn) && !string.IsNullOrEmpty(dataTable.sortColumnDirection))
                {
                    // merchants = merchants.OrderBy(dataTable.sortColumn + " " + dataTable.sortColumnDirection);
                    if (dataTable.sortColumnDirection.ToLower() == "asc")
                        exams = exams.OrderBy(p => EF.Property<object>(p, dataTable.sortColumn));
                    else
                        exams = exams.OrderByDescending(p => EF.Property<object>(p, dataTable.sortColumn));
                }
                recordsTotal = exams.Count();
                //Paging 
                var data = exams.Skip(dataTable.skip).Take(dataTable.pageSize).Select(o => new
                {
                    o.Id,
                    o.Title,
                    o.Paragraph,
                    DateCreated = ((DateTime)o.DateCreated).ToString("yyyy/MM/dd HH:mm"),
                    o.DateModified

                });

                // var ss = data.ToList();
                result.Success = true;
                result.Data = new JsonResult(new { draw = dataTable.draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                result.Success = false;
                // todo log error to database
            }
            return result;

        }

        public Result<Exam> GetExam(long id)
        {
            Result<Exam> result = new Result<Exam>();
            try
            {
                var includes = new List<Expression<Func<Exam, object>>>
                {
                    e => e.Questions
                };
                result.Data = _repository.Exam.Get(includes: includes, predicate: x => !x.IsDeleted).FirstOrDefault();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
            }
            return result;
        }

        public Result<List<Question>> GetExamQuestions(long id)
        {
            Result<List<Question>> result = new Result<List<Question>>();
            try
            {
                result.Data = _repository.Question.Get().Where(a=>a.ExamId == id).ToList();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
            }
            return result;
        }
    }
}
