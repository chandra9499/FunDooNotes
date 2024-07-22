using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models.DTOs.Note
{
    public class UpdateNoteDTO
    {
        public string NoteId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
