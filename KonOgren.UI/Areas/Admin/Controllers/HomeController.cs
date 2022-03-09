
using HtmlAgilityPack;
using KonOgren.DataAccess;
using KonOgren.Domain.Dto;
using KonOgren.Domain.Enum;
using KonOgren.Domain.Model;
using KonOgren.Infrastructure.Helper;
using KonOgren.Infrastructure.Result;
using KonOgren.Infrastructure.ViewModel;
using KonOgren.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;

namespace KonOgren.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IExamService _examService;
        private readonly KonOgrenDBContext _context;
        public HomeController(IExamService examService, KonOgrenDBContext context)
        {
            _examService = examService;
            _context = context;
        }
        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult NewExam()
        {
           
    
            string url = "https://www.wired.com/feed";
            var posts = FeedReaderHelper.FetchFeed(url);
          
            return View(posts.Data);
        }
        [HttpPost]
        public Result AddExam(AddExamViewModel model)
        {
            Result result = new Result();


            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    Exam exam = new Exam();
                    exam.Title = model.title;
                    exam.Paragraph = model.body;

                    var newExam = _examService.AddExam(exam);

                    Question question = new Question();
                    question.Answer = (QuestionAnswer)model.Q1Answer;
                    question.QuestionTitle = model.Q1;
                    question.OptionA = model.Q1A;
                    question.OptionB = model.Q1B;
                    question.OptionC = model.Q1C;
                    question.OptionD = model.Q1D;
                    question.ExamId = newExam.Data.Id;
                    _examService.AddQuestion(question);

                    question = new Question();
                    question.Answer = (QuestionAnswer)model.Q2Answer;
                    question.QuestionTitle = model.Q2;
                    question.OptionA = model.Q2A;
                    question.OptionB = model.Q2B;
                    question.OptionC = model.Q2C;
                    question.OptionD = model.Q2D;
                    question.ExamId = newExam.Data.Id;
                    _examService.AddQuestion(question);

                    question = new Question();
                    question.Answer = (QuestionAnswer)model.Q3Answer;
                    question.QuestionTitle = model.Q3;
                    question.OptionA = model.Q3A;
                    question.OptionB = model.Q3B;
                    question.OptionC = model.Q3C;
                    question.OptionD = model.Q3D;
                    question.ExamId = newExam.Data.Id;
                    _examService.AddQuestion(question);

                    question = new Question();
                    question.Answer = (QuestionAnswer)model.Q4Answer;
                    question.QuestionTitle = model.Q4;
                    question.OptionA = model.Q4A;
                    question.OptionB = model.Q4B;
                    question.OptionC = model.Q4C;
                    question.OptionD = model.Q4D;
                    question.ExamId = newExam.Data.Id;
                    _examService.AddQuestion(question);
                    result.Success = true;
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    result.Success = false;
                }

            }

            return result;
        }
        

        [HttpPost]

        public JsonResult LoadExams()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                // Skiping number of Rows count
                var start = Request.Form["start"].FirstOrDefault();
                // Paging Length 10,20
                var length = Request.Form["length"].FirstOrDefault();
                // Sort Column Name
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                // Sort Column Direction ( asc ,desc)
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10,20,50,100)
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;

                DataTableViewModel dataTable = new DataTableViewModel()
                {
                    draw = draw,
                    pageSize = pageSize,
                    skip = skip,
                    searchValue = searchValue,
                    sortColumnDirection = sortColumnDirection,
                    sortColumn = sortColumn
                };
                return _examService.GetAllExamsPagination(dataTable).Data;


            }
            catch (Exception ex)
            {
                // todo log to database
                throw ex;
            }

        }

        public async Task<IActionResult> LoginAsync()
        {
            try
            {
                await HttpContext.SignOutAsync();
                return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }
      
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpGet]
        public IActionResult DeleteExam(Int64 id)
        {
            try
            {
                var result = _examService.DeleteExam(id);
                if (result.Success)
                {
                    TempData["Message"] = "Silme işlemi gerçekleşmiştir!";
                    TempData["success"] = true;
                }
                else {
                    TempData["Message"] = "Silme işlemi gerçekleşmemiştir!";
                    TempData["success"] = false;
                }
                
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Silme işlemi gerçekleşmemiştir!";
                TempData["success"] = false;
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public Result<string> GetUrlContent(string url)
        {
            Result<string> result =new Result<string>();
            try
            {
                using (WebClient web1 = new WebClient())
                {
                    string data = web1.DownloadString(url);
                    var doc = new HtmlDocument();
                    doc.LoadHtml(data);

                    var nodes = doc.DocumentNode.SelectNodes("//div[@class='body__inner-container']");
                    if (nodes.Count > 0)
                        result.Data = nodes.FirstOrDefault().InnerText.Replace("&#x27;","'");
                    else
                        throw new Exception("");

                }
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
