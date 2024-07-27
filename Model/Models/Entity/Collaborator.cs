using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models.Entity
{
    public class Collaborator
    {
        [Key]
        public int CollabId { get; set; }
        public virtual Note? Note { get; set; }
        [ForeignKey("Note")]
        public int NoteId { get; set; }
        public virtual User? User { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public string? Email { get; set; }
    }
}
