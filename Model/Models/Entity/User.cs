using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models.Entity
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required(ErrorMessage = "First name is required.")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required.")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[\w.]+[@][\w]+[.][a-z[A-Z]]{2,3}$")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        public ICollection<Note>? Notes { get; set;}
    }
}
