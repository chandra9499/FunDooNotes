using DataBaseLayer.Interface;
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
        private readonly ICacheDL _cacheDL;

        public NotesDAL(FunDooDataBaseContext context, ICacheDL cacheDL)
        {
            _context = context;
            _cacheDL = cacheDL;
        }

        public async Task<ResponseModel<NoteDTO>> AddNoteAsync(int userId, CreateNoteDTO createNote)
        {
            var note = await _context.Notes.FirstOrDefaultAsync(x => x.Title == createNote.Title);
            if (note != null) 
            {
                return new ResponseModel<NoteDTO>
                {
                    StatusCode = (int)HttpStatusCode.Created,
                    Message = "Note is already present",
                    Data = null,
                    Success = false
                };
            }
            note = new Note()
            {
                Title = createNote.Title,
                Description = createNote.Descreption,
                CreatedAt = DateTime.Now,
                UserId = userId
            };
            await _context.Notes.AddAsync(note);
            _cacheDL.SetData<Note>($"note{createNote.Title}", note , DateTimeOffset.Now);

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

        public async Task<ResponseModel<NoteDTO>> RemoveNoteAsync(int noteId)
        {
            var note = await _context.Notes.FirstOrDefaultAsync(note => note.NoteId.Equals(noteId));
            if (note == null)
            {
                return new ResponseModel<NoteDTO>
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Success = false,
                    Message = $"Note with {noteId} not found",
                    Data = null
                };
            }

            _context.Notes.Remove(note);

            try
            {
                await _context.SaveChangesAsync();
                _cacheDL.RemoveData($"note{noteId}");

                return new ResponseModel<NoteDTO>
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Success = true,
                    Message = $"Note with title {noteId} deleted",
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
                throw new Exception($"Unable to delete note with title {noteId}", ex);
            }
        }

        public async Task<IEnumerable<Note>> RetrieveNotesAsync(int userId)
        {
            var cacheKey = $"notes{userId}";
            
            var cacheData = await _context.Notes.Include(labels => labels.Labels)
                                            .Include(collb => collb.Collaborators)
                                            .Where(note => note.UserId == userId)
                                            .ToListAsync();

            var expiryTime = DateTimeOffset.Now.AddMinutes(30);
            _cacheDL.SetData<IEnumerable<Note>>(cacheKey, cacheData, expiryTime);

            return cacheData;
        }

        public async Task<ResponseModel<NoteDTO>> ModifyNoteAsync(CreateNoteDTO createNote)
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
                _cacheDL.SetData<Note>($"note{createNote.Title}", note, DateTimeOffset.Now.AddSeconds(30));

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

        public async Task<ResponseModel<List<Note>>> RetrieveNoteByIdAsync(int userId, int noteId)
        {
            var cacheKey = $"note{noteId}_{userId}";
            var cacheData = _cacheDL.GetData<List<Note>>(cacheKey);
            if (cacheData != null && cacheData.Any())
            {
                return new ResponseModel<List<Note>>()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Success = true,
                    Message = $"Note with ID {noteId} found",
                    Data = cacheData
                };
            }

            var note = await _context.Notes
                                     .Include(n => n.Collaborators)
                                     .Include(n => n.Labels)
                                     .FirstOrDefaultAsync(n => n.NoteId == noteId && n.UserId == userId);

            if (note != null)
            {
                var noteList = new List<Note> { note };
                _cacheDL.SetData(cacheKey, noteList, DateTimeOffset.Now.AddSeconds(30));

                return new ResponseModel<List<Note>>()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Success = true,
                    Message = $"Note with ID {noteId} found",
                    Data = noteList
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

        public async Task<ResponseModel<NoteDTO>> AssignColourToNoteAsync(int userId, UpdateColourModel updateColour)
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
                _cacheDL.SetData($"note{updateColour.Title}",note, DateTimeOffset.Now.AddSeconds(30));

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

        public async Task<ResponseModel<Labels>> AssignLabelsToNoteAsync(int noteId, LabelRequestModel addLabels)
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
                _cacheDL.SetData($"note{noteId}_labels", note.Labels.ToList(), DateTimeOffset.Now.AddSeconds(30));

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

        public async Task<ResponseModel<Labels>> UnassignLabelsFromNoteAsync(int noteId, string labelName)
        {
            var note = await _context.Notes.Include(label => label.Labels).FirstOrDefaultAsync(note => note.NoteId.Equals(noteId));
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

            var label = note.Labels.FirstOrDefault(label => label.LabelsName.Equals(labelName));
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

            note.Labels.Remove(label);
            _context.Labels.Remove(label);

            try
            {
                await _context.SaveChangesAsync();
                _cacheDL.SetData($"note{noteId}_labels", note.Labels.ToList(), DateTimeOffset.Now.AddSeconds(30));

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
