using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OCTOBER.Shared.DTO
{
    public  class CourseDTO
    {
        [Precision(8)]
        public int CourseNo { get; set; }
        [StringLength(50)]
        public string Description { get; set; } = null!;
        public decimal? Cost { get; set; }
        [Precision(8)]
        public int? Prerequisite { get; set; }
        [StringLength(30)]
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        [StringLength(30)]
        [Unicode(false)]
        public string ModifiedBy { get; set; } = null!;
        public DateTime ModifiedDate { get; set; }


    }
}
