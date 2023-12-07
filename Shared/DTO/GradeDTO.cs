﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCTOBER.Shared.DTO
{
    public class GradeDTO
    {

        [Precision(8)]
        public int SchoolId { get; set; }

        [Precision(8)]
        public int StudentId { get; set; }

        [Precision(8)]
        public int SectionId { get; set; }

        [StringLength(2)]
        [Unicode(false)]
        public string GradeTypeCode { get; set; } = null!;

        [Precision(3)]
        public byte GradeCodeOccurrence { get; set; }
        public decimal NumericGrade { get; set; }
        public string? Comments { get; set; }
        [StringLength(30)]
        [Unicode(false)]
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        [StringLength(30)]
        [Unicode(false)]
        public string ModifiedBy { get; set; } = null!;
        public DateTime ModifiedDate { get; set; }
    }
}
