using DataBaseLogicLayer.Context;
using DataBaseLogicLayer.Interface;
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
        public ResponseModel<NoteDTO> CreateNote(int userId, CreateNoteDTO createNote)
        {
            var note = new Note()
            {
                Title = createNote.Title,
                Description = createNote.Descreption,
                CreatedAt = DateTime.Now,
                UserId = userId
            };
            _context.Notes.Add(note);
            try
            {
                _context.SaveChanges();
                return new ResponseModel<NoteDTO>
                {
                    StatusCode = (int)HttpStatusCode.Created,
                    Message = "Notes Created",
                    Data = new NoteDTO()
                    {
                        Title = note.Title,
                        Description = note.Description,
                        CreatedAt = DateTime.Now,
                        UserId = note.UserId
                    }
                };
            }
            catch (Exception ex)
            {
                throw new Exception("unable to create notes", ex);
            }

        }

        public ResponseModel<NoteDTO> DeleteNote(string title)
        {
            var notes = _context.Notes.Where(notes => notes.Title.Equals(title)).FirstOrDefault();
            _context.Notes.Remove(notes);
            try
            {
                _context.SaveChanges();
                return new ResponseModel<NoteDTO>
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Success = true,
                    Message = $"note with {title} is deleted",
                    Data = new NoteDTO()
                    {
                        //NoteId=notes.NoteId,
                        Title = notes.Title,
                        CreatedAt = notes.CreatedAt,
                        Description = notes.Description,
                        UserId = notes.UserId
                    }
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"note with {title} is not deleted", ex);
            }
        }

        public IEnumerable<Note> GetNotes(int userId)
        {
            return _context.Notes.ToList();
        }

        public ResponseModel<NoteDTO> UpdateNote(CreateNoteDTO createNote)
        {
            var notes = _context.Notes.FirstOrDefault(note => note.Title.Equals(createNote.Title));
            if (notes != null)
            {
                notes.Description = createNote.Descreption;
                notes.UpdatedAt = DateTime.Now;
                try
                {
                    _context.SaveChanges();
                    return new ResponseModel<NoteDTO>
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                        Success = true,
                        Message = "updation successfull",
                        Data = new NoteDTO()
                        {
                            //NoteId = notes.NoteId,
                            Title = notes.Title,
                            CreatedAt = notes.CreatedAt,
                            Description = notes.Description,
                            UpdatedAt = notes.UpdatedAt,
                            UserId = notes.UserId
                        }
                    };
                }
                catch (Exception ex)
                {
                    throw new Exception("updation unsuccessfull", ex);
                }
            }
            return new ResponseModel<NoteDTO>
            {
                StatusCode = (int)HttpStatusCode.NotModified,
                Success = false,
                Message = $"notes with the {createNote.Title} is not present",
                Data = null
            };

        }

        public ResponseModel<NoteDTO> GetNoteByTitle(int userId, string title)
        {
            var userNotes = GetNotes(userId).ToList();
            if (userNotes != null)
            {
                var note = userNotes.FirstOrDefault(note => note.Title.Equals(title));
                if (note != null)
                {
                    return new ResponseModel<NoteDTO>()
                    {
                        StatusCode = (int)HttpStatusCode.Found,
                        Success = true,
                        Message = $"the note with the title {title} is",
                        Data = new NoteDTO()
                        {
                            //NoteId = note.NoteId,
                            Title = note.Title,
                            CreatedAt = note.CreatedAt,
                            Description = note.Description,
                            UpdatedAt = note.UpdatedAt,
                            UserId = note.UserId
                        }
                    };

                }
                return new ResponseModel<NoteDTO>()
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Success = false,
                    Message = $"the note with the title {title} is not present",
                    Data = null
                };
            }
            return new ResponseModel<NoteDTO>
            {
                StatusCode = (int)HttpStatusCode.OK,
                Success = false,
                Message = "the user does not have any note",
                Data = null
            };
        }

        public ResponseModel<NoteDTO> AddColourToNote(int userId, UpdateColourModel updateColour)
        {
            var userNotes = GetNotes(userId).ToList();
            if (userNotes != null)
            {
                var note = userNotes.FirstOrDefault(note => note.Title.Equals(updateColour.Title));
                if (note != null)
                {
                    note.Colour = updateColour.Colour;
                    _context.SaveChanges();
                    return new ResponseModel<NoteDTO>()
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                        Success = true,
                        Message = $"colour added to the note with title:-{updateColour.Title}",
                        Data = new NoteDTO()
                        {
                            Title = note.Title,
                            colour = note.Colour,
                            CreatedAt = note.CreatedAt,
                            UpdatedAt = DateTime.Now,
                            Description = note.Description,
                            UserId = note.UserId
                        }
                    };
                }
                return new ResponseModel<NoteDTO>()
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Success = false,
                    Message = $"the note with title:-{updateColour.Title} is not present",
                    Data = null
                };
            }
            return new ResponseModel<NoteDTO>()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Success = false,
                Message = $"the user doesn't have any notes",
                Data = null
            };
        }

        
    }
}
