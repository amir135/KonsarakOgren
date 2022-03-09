using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KonOgren.Domain.Model
{
    public class Exam : BaseEntity
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Paragraph { get; set; }
        public List<Question> Questions { get; set; }

    }

}
