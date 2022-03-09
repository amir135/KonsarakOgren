using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KonOgren.Domain.Model
{
    public class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 Id { get; set; }

        [Required]
        [Column("DateCreated")]
        public DateTime DateCreated { get; set; }

        [Column("UserCreated")]
        public Int64? UserCreated { get; set; }

        [Column("DateModified")]
        public DateTime DateModified { get; set; }

        [Column("UserModified")]
        public Int64? UserModified { get; set; }
        [Required]
        [Column("IsActive")]
        public bool IsActive { get; set; }
        [Required]
        [Column("IsDeleted")]
        public bool IsDeleted { get; set; }
    }
}
