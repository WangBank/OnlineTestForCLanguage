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

namespace OnlineTestForCLanguage.Sessions
{
    [AbpAuthorize(PermissionNames.Pages_Papers)]
    public class PapersAppService : AsyncCrudAppService<Paper, PaperDto, long,PagedPaperResultRequestDto,CreatePaperDto,PaperDto>, IPapersAppService
    {
        private readonly IRepository<Paper,long> _PaperRepository;
        public PapersAppService(IRepository<Paper, long> PaperRepository) : base(PaperRepository)
        {
            _PaperRepository = PaperRepository;
        }

        public async Task<PaperDto> GetAsync(PaperDto input)
        {
            var paper = await _PaperRepository.GetAllIncluding(p => p.PaperDetails).FirstOrDefaultAsync(p=>p.Id == input.Id);
            var result =  MapToEntityDto(paper);
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

            var Paper = await _PaperRepository.FirstOrDefaultAsync(input.Id);

            MapToEntity(input, Paper);

            return await GetAsync(input);
        }

        protected override Paper MapToEntity(CreatePaperDto createInput)
        {
           
            var result = new Paper { 
                Score = createInput.Score,
                CreateUserId = AbpSession.UserId.Value,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                Title = createInput.Title,
            };

            return result;
        }


        protected override PaperDto MapToEntityDto(Paper paper)
        {
            var paperDto = base.MapToEntityDto(paper);
            paperDto.PaperDetails = paper.PaperDetails.Select(p=>new PaperDetailDto { 
                ExamId = p.ExamId,
                Id = p.Id,
                IsDeleted = p.IsDeleted,
                Paper = p.Paper,
                PaperId = p.PaperId
            }).ToList();
            return paperDto;
        }

        public async Task DeleteAsync(long id)
        {
            var Paper = await _PaperRepository.FirstOrDefaultAsync(id);
            await _PaperRepository.DeleteAsync(Paper);
        }
    }
}
