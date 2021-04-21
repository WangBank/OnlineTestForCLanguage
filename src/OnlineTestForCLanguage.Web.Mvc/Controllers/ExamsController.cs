using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTestForCLanguage.Authorization;
using OnlineTestForCLanguage.Controllers;
using OnlineTestForCLanguage.Exams.Dto;
using OnlineTestForCLanguage.Sessions;
using OnlineTestForCLanguage.Sessions.Dto;
using OnlineTestForCLanguage.Web.Models.Exams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTestForCLanguage.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Exams, PermissionNames.Pages_Papers, PermissionNames.Pages_TestCounts, PermissionNames.Pages_Tests)]
    public class ExamsController : OnlineTestForCLanguageControllerBase
    {
        private readonly IExamsAppService _examAppService;

        public ExamsController(IExamsAppService examAppService)
        {
            _examAppService = examAppService;
        }

        public async Task<ActionResult> Index(PagedExamResultRequestDto input)
        {
            var output = await _examAppService.GetAllAsync(input);
            var model = new IndexViewModel(output.Items);
            return View(model);
        }


        public async Task<ActionResult> EditModal(long examId)
        {
            var exam = await _examAppService.GetAsync(new EntityDto<long>(examId));
            var model = new EditExamModalViewModel();
            model.Exam = exam;
            return PartialView("_EditModal", model);
        }
    }
}
