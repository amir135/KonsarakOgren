using KonOgren.Infrastructure.Result;
using KonOgren.Infrastructure.ViewModel;
using KonOgren.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KonOgren.UI.Controllers
{

    public class HomeController : Controller
    {


        private readonly IExamService _examService;
        public HomeController(IExamService examService)
        {
            _examService = examService;
        }
 

        public IActionResult Index()
        {
            var result = _examService.GetAllExams();
            return View(result.Data);
        }
        public IActionResult startexam(Int64 id)
        {
            var result = _examService.GetExam(id);
            return View(result.Data);
        }


        [HttpPost]

        public JsonResult LoadExam()
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



        [HttpPost]
        public Result<CheckResultViewModel> CheckResult(CheckResultViewModel model)
        {
            Result<CheckResultViewModel> result = new Result<CheckResultViewModel>();


            try
            {
                var exam = _examService.GetExam(model.Id);
                if (exam != null)
                {
                    CheckResultViewModel res = new CheckResultViewModel();
                    res.q1 = ((int)exam.Data.Questions.FirstOrDefault(a => a.Id == model.q1Id).Answer).ToString();
                    res.q2 = ((int)exam.Data.Questions.FirstOrDefault(a => a.Id == model.q2Id).Answer).ToString();
                    res.q3 = ((int)exam.Data.Questions.FirstOrDefault(a => a.Id == model.q3Id).Answer).ToString();
                    res.q4 = ((int)exam.Data.Questions.FirstOrDefault(a => a.Id == model.q4Id).Answer).ToString();
                    result.Data = res;
                    result.Success = true;
                }
                else
                    throw new Exception();
            }
            catch (Exception ex)
            {

                result.Success = false;
            }

            return result;
        }
    }
}
