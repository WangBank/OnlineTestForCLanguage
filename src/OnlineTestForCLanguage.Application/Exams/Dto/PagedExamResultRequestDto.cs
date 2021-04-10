﻿using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTestForCLanguage.Exams.Dto
{
    public class PagedExamResultRequestDto : PagedResultRequestDto
    {
        public ExamType? ExamType { get; set; }
        public DifficultyType? Difficulty { get; set; }
    }
}