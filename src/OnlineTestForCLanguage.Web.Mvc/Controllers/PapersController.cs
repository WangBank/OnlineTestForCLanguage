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
    [AbpMvcAuthorize(PermissionNames.Pages_Papers)]
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
            var exams = await _ExamsAppService.GetAllAsync(input);
            var model = new IndexPaperModalViewModel { Exams = exams.Items };
            return View(model);
        }
        public async Task<ActionResult> EditModal(long PaperId)
        {
            var Paper = await _PaperAppService.GetAsync(new EntityDto<long> { Id = PaperId});
            var model = new EditPaperModalViewModel();
            var exams = await _ExamsAppService.GetAllAsync(new PagedExamResultRequestDto());
            model.Exams = exams.Items;
            model.Paper = Paper;
            return PartialView("_EditModal", model);
        }
    }
}
