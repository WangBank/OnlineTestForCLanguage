using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using OnlineTestForCLanguage.Authorization;
using OnlineTestForCLanguage.Exams;
using OnlineTestForCLanguage.Exams.Dto;
using OnlineTestForCLanguage.Sessions.Dto;

namespace OnlineTestForCLanguage.Sessions
{
    [AbpAuthorize(PermissionNames.Pages_Exams)]
    public class ExamsAppService : AsyncCrudAppService<Exam, ExamDto, int,PagedExamResultRequestDto,CreateExamDto,ExamDto>, IExamsAppService
    {
        private readonly IRepository<Exam> _examRepository;
        public ExamsAppService(IRepository<Exam> examRepository) : base(examRepository)
        {
            _examRepository = examRepository;
        }
        public async Task<ListResultDto<ExamDto>> GetExamsAsync(PagedExamResultRequestDto input)
        {
            var exams = await GetAllAsync(input);
            exams.Items = exams.Items.OrderBy(e=>e.Id).ToList();
            return exams;
        }

        public override async Task<ExamDto> CreateAsync(CreateExamDto input)
        {
            CheckCreatePermission();

            var exam = ObjectMapper.Map<Exam>(input);

          
            await _examRepository.InsertOrUpdateAsync(exam);

            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(exam);
        }

        public override async Task<ExamDto> UpdateAsync(ExamDto input)
        {
            CheckUpdatePermission();

            var exam = await _examRepository.FirstOrDefaultAsync(input.Id);

            MapToEntity(input, exam);

            return await GetAsync(input);
        }

        public override async Task DeleteAsync(EntityDto<int> input)
        {
            var exam = await _examRepository.FirstOrDefaultAsync(input.Id);
            await _examRepository.DeleteAsync(exam);
        }
    }
}
