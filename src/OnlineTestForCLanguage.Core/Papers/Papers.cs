using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTestForCLanguage.Papers
{
    [Table("Papers")]
    public class Paper : Entity<long>, IHasCreationTime, ISoftDelete
    {
        
        public string Title { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<PaperDetail> PaperDetails { get; set; }
        public decimal Score { get; set; }
        public long CreateUserId { get; set; }
        public Paper()
        {
            IsDeleted = false;
            CreationTime = DateTime.Now;
        }
    }

    [Table("PaperDetails")]
    public class PaperDetail : Entity<long>, ISoftDelete
    {
        public long PaperId { get; set; }
        [ForeignKey("PaperId")]
        public Paper Paper { get; set; }
        public long ExamId { get; set; }

        public bool IsDeleted { get; set; }
        public PaperDetail()
        {
            IsDeleted = false;

        }
    }

}
