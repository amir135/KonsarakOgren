using KonOgren.Domain.Dto;
using KonOgren.Domain.Model;
using KonOgren.Infrastructure.Result;
using KonOgren.Infrastructure.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace KonOgren.Services
{
    public interface IExamService
    {
        Result<List<Exam>> GetAllExams();
        Result<Exam> GetExam(Int64 id);
        Result<List<Question>> GetExamQuestions(Int64 id);
        Result<JsonResult> GetAllExamsPagination(DataTableViewModel dataTable);
        Result<Exam> AddExam(Exam model);
        Result<Question> AddQuestion(Question model);
        Result DeleteExam(Int64 userId);
    }
}
