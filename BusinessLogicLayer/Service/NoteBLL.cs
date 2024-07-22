using BusinessLogicLayer.Interface;
using DataBaseLogicLayer.Interface;
using Model.Models.DTOs.Note;
using Model.Models.Entity;
using Model.Models.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Service
{
    public class NoteBLL : INoteBLL
    {
        private readonly INotesDAL _notesDAL;
        public NoteBLL(INotesDAL notesDAL)
        {
            _notesDAL = notesDAL;
        }
        public ResponseModel<NoteDTO> CreateNote(int userId, CreateNoteDTO createNote)
        {
            return _notesDAL.CreateNote(userId, createNote);
        }

        public ResponseModel<NoteDTO> DeleteNote(string title)
        {
            return _notesDAL.DeleteNote(title);
        }

        public IEnumerable<Note> GetNotes(int userId)
        {
            return _notesDAL.GetNotes(userId);
        }

        public ResponseModel<NoteDTO> UpdateNote(CreateNoteDTO createNote)
        {
            return _notesDAL.UpdateNote(createNote); D:\ASP.NET\Entity frame work projects\FunDooNotes\FunDooNotes\FunDooNotes.sln
        }
    }
}
