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
    public class Note
    {
        [Key]
        public int NoteId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Colour { get; set; } = string.Empty;
        public bool IsArchived { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        //[JsonIgnore]
        //one note has many collaborators
        public ICollection<Collaborator>? Collaborators { get; set; }
        //[JsonIgnore]
        //$ One Note Can Have Many Labels 
        public ICollection<Labels>? Labels { get; set; }
        [ForeignKey("Users")]
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
