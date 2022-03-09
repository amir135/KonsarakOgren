using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KonOgren.Domain.Dto
{
    public class UserDto
    {
        [Required]
        public Int64 Id { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public bool IsAutomaticEmail { get; set; }

    }
}
