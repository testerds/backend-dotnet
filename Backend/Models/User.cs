using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class User
    {
        public Guid? UserId { get; set; }
        [Required]
        public DateTime? CreationDate { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        public string Matricula { get; set; }
        public string Faculdade { get; set; }
        [Required]
        public bool IsTeacher { get; set; }
    }
}
