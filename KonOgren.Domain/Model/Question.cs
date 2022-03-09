using KonOgren.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KonOgren.Domain.Model
{
    public class Question: BaseEntity
    {
        public string QuestionTitle { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        [ForeignKey("Exam")]
        public Int64 ExamId { get; set; }

        public QuestionAnswer Answer { get; set; }

        public Exam Exam { get; set; }

    }
}
