using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using OnlineTestForCLanguage.Authorization;
using OnlineTestForCLanguage.Papers;
using OnlineTestForCLanguage.Papers.Dto;
using OnlineTestForCLanguage.Sessions.Dto;
using OnlineTestForCLanguage.Authorization.Users;
namespace OnlineTestForCLanguage.Sessions
{
    [AbpAuthorize(PermissionNames.Pages_Exams, PermissionNames.Pages_Papers, PermissionNames.Pages_TestCounts, PermissionNames.Pages_Tests)]
    public class PapersAppService : AsyncCrudAppService<Paper, PaperDto, long,PagedPaperResultRequestDto,CreatePaperDto,PaperDto>, IPapersAppService
    {
        private readonly IRepository<Paper,long> _PaperRepository;
        private readonly UserManager _userManager;
        public PapersAppService(IRepository<Paper, long> PaperRepository, UserManager userManager) : base(PaperRepository)
        {
            _PaperRepository = PaperRepository;
            _userManager = userManager;
        }
        public override async Task<PaperDto> GetAsync(EntityDto<long> input)
        {
            var paper = await _PaperRepository.GetAllIncluding(p => p.PaperDetails).FirstOrDefaultAsync(p => p.Id == input.Id);
            var result = MapToEntityDto(paper);
            return result;
        }

        public async Task<ListResultDto<PaperDto>> GetPapersAsync(PagedPaperResultRequestDto input)
        {
            var Papers = await GetAllAsync(input);
            Papers.Items = Papers.Items.OrderBy(e=>e.Id).ToList();
            return Papers;
        }

        public override async Task<PaperDto> CreateAsync(CreatePaperDto input)
        {
            CheckCreatePermission();

            var entity = MapToEntity(input);

          
            await _PaperRepository.InsertOrUpdateAsync(entity);

            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(entity);
        }
        public override async Task<PaperDto> UpdateAsync(PaperDto input)
        {
            CheckUpdatePermission();

            var paper = await _PaperRepository.GetAllIncluding(p => p.PaperDetails).FirstOrDefaultAsync(p => p.Id == input.Id);

            MapToEntity(input, paper);

            return MapToEntityDto(paper);
        }


        protected override void MapToEntity(PaperDto input, Paper paper)
        {
            paper.Score = input.Score;
            paper.Title = input.Title;
            paper.PaperDetails = input.ExamList.Select(p => new PaperDetail
            {
                ExamId = p,
                IsDeleted = false,
            }).ToList();
        }

        protected override Paper MapToEntity(CreatePaperDto createInput)
        {

            var result = new Paper {
                Score = createInput.Score,
                CreateUserId = AbpSession.UserId.Value,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                Title = createInput.Title,
                PaperDetails = createInput.ExamList.Select(p=>new PaperDetail { 
                    ExamId = p,
                    IsDeleted = false,
                }).ToList()
            };

            return result;
        }


        protected override PaperDto MapToEntityDto(Paper paper)
        {
            var paperDto = new PaperDto { 
                Score = paper.Score,
                CreateUserId = paper.CreateUserId,
                CreateUserName =_userManager.GetUserById(paper.CreateUserId).UserName,
                CreationTime = paper.CreationTime,
                Id = paper.Id,
                IsDeleted = paper.IsDeleted,
                PaperDetails =new List<PaperDetailDto>() ,
                Title =paper.Title 
            };
            if (paper.PaperDetails  != null&& paper.PaperDetails.Count != 0)
            {
                paperDto.PaperDetails = paper.PaperDetails.Select(p => new PaperDetailDto
                {
                    ExamId = p.ExamId,
                    Id = p.Id,
                    IsDeleted = p.IsDeleted,
                    PaperId = p.PaperId
                }).ToList();
            }
            
            return paperDto;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var Paper = await _PaperRepository.FirstOrDefaultAsync(input.Id);
            await _PaperRepository.DeleteAsync(Paper);
        }
    }
}
