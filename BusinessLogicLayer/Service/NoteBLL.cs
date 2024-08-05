using BusinessLogicLayer.Interface;
using DataBaseLogicLayer.Interface;
using Model.Models.DTOs.Labels;
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

        public async Task<ResponseModel<NoteDTO>> AddColourToNoteAsync(int userId, UpdateColourModel updateColour)
        {
            return await _notesDAL.AssignColourToNoteAsync(userId,updateColour);
        }

        public async Task<ResponseModel<Labels>> AddLabelsToNotesAsync(int noteId, LabelRequestModel addLabels)
        {
            return await _notesDAL.AssignLabelsToNoteAsync(noteId, addLabels);
        }

        public async Task<ResponseModel<NoteDTO>> CreateNoteAsync(int userId, CreateNoteDTO createNote)
        {
            return await _notesDAL.AddNoteAsync(userId,createNote);
        }

        public async Task<ResponseModel<NoteDTO>> DeleteByNoteIdAsync(int noteId)
        {
            return await _notesDAL.RemoveNoteAsync(noteId);
        }

        public async Task<ResponseModel<List<Note>>> GetNoteByIdAsync(int userId, int noteId)
        {
            return await _notesDAL.RetrieveNoteByIdAsync(userId, noteId);
        }

        public async Task<IEnumerable<Note>> GetNotesAsync(int userId)
        {
            return await _notesDAL.RetrieveNotesAsync(userId);
        }

        public async Task<ResponseModel<NoteDTO>> UpdateNoteAsync(CreateNoteDTO createNote)
        {
            return await _notesDAL.ModifyNoteAsync(createNote);
        }
    }
}
