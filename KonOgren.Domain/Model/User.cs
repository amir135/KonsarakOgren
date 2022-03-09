using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace KonOgren.Domain.Model
{
   public class User : BaseEntity
    {
        [Index("IX_UserEmail", 1, IsUnique = true)]
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

       // [Required]
        [MaxLength(500)]
        public string Password { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Surname { get; set; }

    }
}