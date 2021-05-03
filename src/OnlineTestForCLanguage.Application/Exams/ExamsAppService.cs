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
    [AbpAuthorize(PermissionNames.Pages_Exams, PermissionNames.Pages_Papers, PermissionNames.Pages_TestCounts, PermissionNames.Pages_Tests)]
    public class ExamsAppService : AsyncCrudAppService<Exam, ExamDto, long,PagedExamResultRequestDto,CreateExamDto,ExamDto>, IExamsAppService
    {
        private readonly IRepository<Exam, long> _examRepository;
        public ExamsAppService(IRepository<Exam, long> examRepository) : base(examRepository)
        {
            _examRepository = examRepository;
        }
        public override async Task<ExamDto> CreateAsync(CreateExamDto input)
        {
            CheckCreatePermission();

            var exam = MapToEntity(input);

          
            await _examRepository.InsertOrUpdateAsync(exam);

            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(exam);
        }
        protected override Exam MapToEntity(CreateExamDto createInput)
        {

            var result = new Exam
            {
               Score = createInput.Score,
               Content = createInput.Content,
               CorrectDetailIds = createInput.CorrectAnswerId,
               CreationTime = createInput.CreationTime,
               Difficulty = createInput.Difficulty,
               ExamDetails = createInput.answers.Select(e=>new ExamDetail { 
                Content = e.Content,
                CreationTime = createInput.CreationTime,
                IsDeleted = false,
                AnswerId = e.AnswerId,
               }).ToList(),
               ExamType = createInput.ExamType,
               Explain = createInput.Explain,
               IsDeleted = false,
               Title = createInput.Title
            };

            return result;
        }
        public override async Task<ExamDto> UpdateAsync(ExamDto input)
        {
            CheckUpdatePermission();

            var exam = await _examRepository.GetAllIncluding(s=>s.ExamDetails).FirstOrDefaultAsync(e=>e.Id == input.Id);

            MapToEntity(input, exam);

            return await GetAsync(input);
        }
        public override async Task<ExamDto> GetAsync(EntityDto<long> input)
        {
            var exam = await _examRepository.GetAllIncluding(p => p.ExamDetails).FirstOrDefaultAsync(p => p.Id == input.Id);
            var result = MapToEntityDto(exam);
            return result;
        }
        protected override ExamDto MapToEntityDto(Exam exam)
        {
            var examDto = new ExamDto
            {
               Score = exam.Score,
               Content = exam.Content,
               CorrectDetailIds = exam.CorrectDetailIds,
               CreationTime = exam.CreationTime,
               Difficulty = exam.Difficulty,
               ExamType = exam.ExamType,
               Explain = exam.Explain,
               Id = exam.Id,
               IsDeleted = exam.IsDeleted,
               Title = exam.Title
            };
            if (exam.ExamDetails != null && exam.ExamDetails.Count != 0)
            {
                switch (exam.ExamType)
                {
                    case ExamType.SingleSelect:
                        examDto.answers = exam.ExamDetails.Select(e => new ExamDetailDto
                        {
                            IsSelected = e.AnswerId == exam.CorrectDetailIds ? true : false,
                            AnswerId = e.AnswerId,
                            Content = e.Content,
                            Id = e.Id
                        }).ToList();
                        break;
                    case ExamType.MulSelect:
                        examDto.answers = exam.ExamDetails.Select(e => new ExamDetailDto
                        {
                            IsSelected = exam.CorrectDetailIds.Split(",").Contains(e.AnswerId) ? true : false,
                            AnswerId = e.AnswerId,
                            Content = e.Content,
                            Id = e.Id
                        }).ToList();
                        break;
                    case ExamType.Judge:
                        examDto.answers = exam.ExamDetails.Select(e => new ExamDetailDto
                        {
                            IsSelected = e.AnswerId == exam.CorrectDetailIds ? true : false,
                            AnswerId = e.AnswerId,
                            Content = e.Content,
                            Id = e.Id
                        }).ToList();
                        break;
                    case ExamType.ShortAnswer:
                        examDto.answers = exam.ExamDetails.Select(e => new ExamDetailDto
                        {
                            IsSelected = true,
                            AnswerId = e.AnswerId,
                            Content = e.Content,
                            Id = e.Id
                        }).ToList();
                        break;
                    default:
                        break;
                }
            }
            
            return examDto;
        }
        protected override void MapToEntity(ExamDto input, Exam entity)
        {
            entity.Title = input.Title;
            entity.Content = input.Content;
            entity.CorrectDetailIds = input.CorrectDetailIds;
            entity.Difficulty = input.Difficulty;
            entity.Explain = input.Explain;
            if (entity.ExamDetails != null && input.answers != null)
            {
                foreach (var item in entity.ExamDetails)
                {
                    item.Content = input.answers.FirstOrDefault(e => e.AnswerId == item.AnswerId).Content;
                }
            }
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var exam = await _examRepository.FirstOrDefaultAsync(input.Id);
            await _examRepository.DeleteAsync(exam);
        }
    }
}
