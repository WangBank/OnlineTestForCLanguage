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


        /// <summary>
        /// 获取试题列表，带分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override async Task<PagedResultDto<ExamDto>> GetAllAsync(PagedExamResultRequestDto input)
        {
            var count = _examRepository
                .GetAll()
                .Where(e => !e.IsDeleted)
                .WhereIf(input.ExamTypeSearch.HasValue && input.ExamTypeSearch.Value != ExamType.all, e => e.ExamType == input.ExamTypeSearch.Value)
                .WhereIf(input.Difficulty.HasValue && input.Difficulty.Value != DifficultyType.all, e => e.Difficulty == input.Difficulty.Value)
                .WhereIf(!string.IsNullOrEmpty(input.Keyword), e => e.Title.Contains(input.Keyword) || e.Content.Contains(input.Keyword))
                .Count();
            var exams =await _examRepository
                .GetAll()
                .Where(e=> !e.IsDeleted)
                .WhereIf(input.ExamTypeSearch.HasValue && input.ExamTypeSearch.Value != ExamType.all, e => e.ExamType == input.ExamTypeSearch.Value)
                .WhereIf(input.Difficulty.HasValue && input.Difficulty.Value != DifficultyType.all, e => e.Difficulty == input.Difficulty.Value)
                .WhereIf(!string.IsNullOrEmpty(input.Keyword), e => e.Title.Contains(input.Keyword) || e.Content.Contains(input.Keyword))
                .Skip(input.SkipCount).Take(input.MaxResultCount)
                .ToListAsync();
            var result = new PagedResultDto<ExamDto> { 
                TotalCount = count,
                Items = exams.Select(s=>MapToEntityDto(s)).ToList()
            };
            return result;
        }

        /// <summary>
        /// 创建试题
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 修改考题
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override async Task<ExamDto> UpdateAsync(ExamDto input)
        {
            CheckUpdatePermission();

            var exam = await _examRepository.GetAllIncluding(s=>s.ExamDetails).FirstOrDefaultAsync(e=>e.Id == input.Id);

            MapToEntity(input, exam);

            return await GetAsync(input);
        }

        /// <summary>
        /// 获取单个考题明细
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override async Task<ExamDto> GetAsync(EntityDto<long> input)
        {
            var exam = await _examRepository.GetAllIncluding(p => p.ExamDetails).FirstOrDefaultAsync(p => p.Id == input.Id && !p.IsDeleted);
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

       /// <summary>
       /// 删除考题
       /// </summary>
       /// <param name="input"></param>
       /// <returns></returns>
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var exam = await _examRepository.FirstOrDefaultAsync(input.Id);
            await _examRepository.DeleteAsync(exam);
        }

        /// <summary>
        /// 获取不带分页的考题列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<ExamDto>> GetAllNoPageAsync()
        {
            var exams = await _examRepository.GetAllIncluding(p => p.ExamDetails).Where(p=>!p.IsDeleted).ToListAsync();
            var result = exams.Select(e=>MapToEntityDto(e)).ToList();
            return result;
        }
    }
}
