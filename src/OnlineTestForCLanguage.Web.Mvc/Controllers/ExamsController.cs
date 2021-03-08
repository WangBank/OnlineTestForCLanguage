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
    [AbpMvcAuthorize(PermissionNames.Pages_Exams)]
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
    }
}
