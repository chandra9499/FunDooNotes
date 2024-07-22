using Model.Models.DTOs.Note;
using Model.Models.Entity;
using Model.Models.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLogicLayer.Interface
{
    public interface INotesDAL
    {
        ResponseModel<NoteDTO> CreateNote(int userId, CreateNoteDTO createNote);
        IEnumerable<Note> GetNotes(int userId);
        ResponseModel<NoteDTO> UpdateNote(CreateNoteDTO createNote);
        ResponseModel<NoteDTO> DeleteNote(string title);
    }
}
