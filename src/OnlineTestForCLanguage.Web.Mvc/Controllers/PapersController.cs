using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTestForCLanguage.Authorization;
using OnlineTestForCLanguage.Controllers;
using OnlineTestForCLanguage.Papers.Dto;
using OnlineTestForCLanguage.Exams.Dto;
using OnlineTestForCLanguage.Sessions;
using OnlineTestForCLanguage.Sessions.Dto;
using OnlineTestForCLanguage.Web.Models.Papers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTestForCLanguage.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Exams, PermissionNames.Pages_Papers, PermissionNames.Pages_TestCounts, PermissionNames.Pages_Tests)]
    public class PapersController : OnlineTestForCLanguageControllerBase
    {
        private readonly IPapersAppService _PaperAppService;
        private readonly IExamsAppService _ExamsAppService;

        public PapersController(IPapersAppService PaperAppService, IExamsAppService ExamsAppService)
        {
            _PaperAppService = PaperAppService;
            _ExamsAppService = ExamsAppService;
        }

        public async Task<ActionResult> Index(PagedExamResultRequestDto input)
        {
            // 单选6 多选3 判断2 简答3
            var exams = await _ExamsAppService.GetAllNoPageAsync();
            if (exams.Where(e=>e.ExamType == Exams.ExamType.Judge).Count() < 2 || exams.Where(e => e.ExamType == Exams.ExamType.ShortAnswer).Count() < 3 || exams.Where(e => e.ExamType == Exams.ExamType.MulSelect).Count() < 3 || exams.Where(e => e.ExamType == Exams.ExamType.SingleSelect).Count() < 6)
            {
                ViewBag.CanAutoCreatePaper = false;
            }
            else
            {
                ViewBag.CanAutoCreatePaper = true;
            }
            var model = new IndexPaperModalViewModel { Exams = exams };
            return View(model);
        }
        public async Task<ActionResult> EditModal(long PaperId)
        {
            var Paper = await _PaperAppService.GetAsync(new EntityDto<long> { Id = PaperId});
            var model = new EditPaperModalViewModel();
            var exams = await _ExamsAppService.GetAllNoPageAsync();
            model.Exams = exams;
            model.Paper = Paper;
            return PartialView("_EditModal", model);
        }
    }
}
