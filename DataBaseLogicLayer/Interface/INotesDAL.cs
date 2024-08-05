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
        Task<ResponseModel<NoteDTO>> AddNoteAsync(int userId, CreateNoteDTO createNote);
        Task<ResponseModel<NoteDTO>> RemoveNoteAsync(int noteId);
        Task<IEnumerable<Note>> RetrieveNotesAsync(int userId);
        Task<ResponseModel<NoteDTO>> ModifyNoteAsync(CreateNoteDTO createNote);
        Task<ResponseModel<List<Note>>> RetrieveNoteByIdAsync(int userId, int noteId);
        Task<ResponseModel<NoteDTO>> AssignColourToNoteAsync(int userId, UpdateColourModel updateColour);
        Task<ResponseModel<Labels>> AssignLabelsToNoteAsync(int noteId, LabelRequestModel addLabels);
        Task<ResponseModel<Labels>> UnassignLabelsFromNoteAsync(int noteId, string labelName);
    }
}
