using Model.Models.DTOs.Labels;
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
        Task<ResponseModel<NoteDTO>> CreateNoteAsync(int userId, CreateNoteDTO createNote);
        Task<IEnumerable<Note>> GetNotesAsync(int userId);
        Task<ResponseModel<NoteDTO>> UpdateNoteAsync(CreateNoteDTO createNote);
        Task<ResponseModel<List<Note>>> GetNoteByIdAsync(int userId, int noteId);
        Task<ResponseModel<NoteDTO>> DeleteNoteAsync(string title);
        Task<ResponseModel<NoteDTO>> AddColourToNoteAsync(int userId, UpdateColourModel updateColour);
        Task<ResponseModel<Labels>> AddLabelsToNotesAsync(int noteId, LabelRequestModel label);
    }
}
