using DataBaseLogicLayer.Context;
using DataBaseLogicLayer.Interface;
using Microsoft.EntityFrameworkCore;
using Model.Models.DTOs.Labels;
using Model.Models.DTOs.Note;
using Model.Models.Entity;
using Model.Models.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLogicLayer.Repository
{
    public class NotesDAL : INotesDAL
    {
        private readonly FunDooDataBaseContext _context;
        public NotesDAL(FunDooDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<NoteDTO>> CreateNoteAsync(int userId, CreateNoteDTO createNote)
        {
            var note = new Note()
            {
                Title = createNote.Title,
                Description = createNote.Descreption,
                CreatedAt = DateTime.Now,
                UserId = userId
            };

            await _context.Notes.AddAsync(note);

            try
            {
                await _context.SaveChangesAsync();
                return new ResponseModel<NoteDTO>
                {
                    StatusCode = (int)HttpStatusCode.Created,
                    Message = "Note Created",
                    Data = new NoteDTO()
                    {
                        Title = note.Title,
                        Description = note.Description,
                        CreatedAt = note.CreatedAt,
                        UserId = note.UserId
                    }
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to create note", ex);
            }
        }

        public async Task<ResponseModel<NoteDTO>> DeleteNoteAsync(string title)
        {
            var note = await _context.Notes.FirstOrDefaultAsync(n => n.Title == title);
            if (note == null)
            {
                return new ResponseModel<NoteDTO>
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Success = false,
                    Message = $"Note with title {title} not found",
                    Data = null
                };
            }

            _context.Notes.Remove(note);

            try
            {
                await _context.SaveChangesAsync();
                return new ResponseModel<NoteDTO>
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Success = true,
                    Message = $"Note with title {title} deleted",
                    Data = new NoteDTO()
                    {
                        Title = note.Title,
                        CreatedAt = note.CreatedAt,
                        Description = note.Description,
                        UserId = note.UserId
                    }
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to delete note with title {title}", ex);
            }
        }

        public async Task<IEnumerable<Note>> GetNotesAsync(int userId)
        {
            return await _context.Notes.Include(labels=>labels.Labels).Include(collb=>collb.Collaborators).Where(note => note.UserId == userId).ToListAsync();
        }

        public async Task<ResponseModel<NoteDTO>> UpdateNoteAsync(CreateNoteDTO createNote)
        {
            var note = await _context.Notes.FirstOrDefaultAsync(n => n.Title == createNote.Title);
            if (note == null)
            {
                return new ResponseModel<NoteDTO>
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Success = false,
                    Message = $"Note with title {createNote.Title} not found",
                    Data = null
                };
            }

            note.Description = createNote.Descreption;
            note.UpdatedAt = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
                return new ResponseModel<NoteDTO>
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Success = true,
                    Message = "Note updated successfully",
                    Data = new NoteDTO()
                    {
                        Title = note.Title,
                        CreatedAt = note.CreatedAt,
                        Description = note.Description,
                        UpdatedAt = note.UpdatedAt,
                        UserId = note.UserId
                    }
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to update note", ex);
            }
        }

        public async Task<ResponseModel<List<Note>>> GetNoteByIdAsync(int userId, int noteId)
        {
            var note = await _context.Notes
                                     .Include(n => n.Collaborators)
                                     .Include(n => n.Labels)
                                     .FirstOrDefaultAsync(n => n.NoteId == noteId && n.UserId == userId);

            if (note != null)
            {
                return new ResponseModel<List<Note>>()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Success = true,
                    Message = $"Note with ID {noteId} found",
                    Data = new List<Note> { note }
                };
            }

            return new ResponseModel<List<Note>>()
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Success = false,
                Message = $"Note with ID {noteId} not found",
                Data = null
            };
        }

        public async Task<ResponseModel<NoteDTO>> AddColourToNoteAsync(int userId, UpdateColourModel updateColour)
        {
            var note = await _context.Notes.FirstOrDefaultAsync(n => n.UserId == userId && n.Title == updateColour.Title);
            if (note == null)
            {
                return new ResponseModel<NoteDTO>()
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Success = false,
                    Message = $"Note with title {updateColour.Title} not found",
                    Data = null
                };
            }

            note.Colour = updateColour.Colour;

            try
            {
                await _context.SaveChangesAsync();
                return new ResponseModel<NoteDTO>()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Success = true,
                    Message = $"Colour added to note with title: {updateColour.Title}",
                    Data = new NoteDTO()
                    {
                        Title = note.Title,
                        Colour = note.Colour,
                        CreatedAt = note.CreatedAt,
                        UpdatedAt = note.UpdatedAt,
                        Description = note.Description,
                        UserId = note.UserId
                    }
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to add colour to note", ex);
            }
        }

        public async Task<ResponseModel<Labels>> AddLabelsToNotesAsync(int noteId, LabelRequestModel addLabels)
        {
            var note = await _context.Notes.Include(n => n.Labels).FirstOrDefaultAsync(n => n.NoteId == noteId);
            if (note == null)
            {
                return new ResponseModel<Labels>
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Success = false,
                    Message = "Note not found",
                    Data = null
                };
            }

            var existingLabel = await _context.Labels.FirstOrDefaultAsync(label => label.LabelsName == addLabels.LabelsName);
            Labels label;

            if (existingLabel != null)
            {
                label = existingLabel;
                if (!note.Labels.Contains(label))
                {
                    note.Labels.Add(label);
                }
            }
            else
            {
                label = new Labels
                {
                    LabelsName = addLabels.LabelsName,
                    LabelsDescription = addLabels.LabelsDescription
                };
                note.Labels.Add(label);
                _context.Labels.Add(label);
            }

            try
            {
                await _context.SaveChangesAsync();
                return new ResponseModel<Labels>
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Success = true,
                    Message = "Label added to note",
                    Data = label
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to add label to note", ex);
            }
        }

        public async Task<ResponseModel<Labels>> RemoveLabelsToNotesAsync(int noteId, String labelName)
        {
            var note = await _context.Notes.Include(label=>label.Labels).FirstOrDefaultAsync(note=>note.NoteId.Equals(noteId));
            if (note == null) 
            {
                return new ResponseModel<Labels>
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Success = false,
                    Message = "Note not found",
                    Data = null
                };
            }
            var label = note.Labels.Where(label=>label.LabelsName.Equals(labelName)).FirstOrDefault();
            if (label == null)
            {
                return new ResponseModel<Labels>
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Success = false,
                    Message = "Note does not have any label",
                    Data = null
                };
            }
            _context.Labels.Remove(label);
            try
            {
                await _context.SaveChangesAsync();
                return new ResponseModel<Labels>
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Success = true,
                    Message = "Label removed from the note",
                    Data = label
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to remove the label from note", ex);
            }


        }

    }
}
