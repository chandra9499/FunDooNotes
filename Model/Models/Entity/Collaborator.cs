using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Model.Models.Entity
{
    public class Collaborator
    {
        [Key]
        public int CollabId { get; set; }
        [JsonIgnore]
        public  Note? Note { get; set; }
        [ForeignKey("Note")]
        public int NoteId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
        [ForeignKey("User")]
        public int CollaboratorUserId { get; set; }
        [Required]
        public string? CollaboratorEmail { get; set; }
    }
}
