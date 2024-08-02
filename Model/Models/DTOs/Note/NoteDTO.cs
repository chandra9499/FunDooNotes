using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models.DTOs.Note
{
    public class NoteDTO
    {
        public int NoteId { get; set; }
        public int UserId { get; set; }
        public string? Title { get; set; }
        public List<string>? Labels { get; set; }
        public string? Description { get; set; }
        public string? Colour { get; set; } 
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
