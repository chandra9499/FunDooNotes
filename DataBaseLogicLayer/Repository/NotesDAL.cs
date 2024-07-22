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
        public ResponseModel<NoteDTO> CreateNote(int userId,CreateNoteDTO createNote)
        {
            var note = new Note()
            {               
                Title=createNote.Title,
                Description=createNote.Descreption,
                CreatedAt = DateTime.Now,
                UserId = userId
            };
            _context.Notes.Add(note);
            try
            {
                _context.SaveChanges();
                return new ResponseModel<NoteDTO> 
                { 
                    StatusCode = (int) HttpStatusCode.Created,
                    Message = "Notes Created",
                    Data =  new NoteDTO()
                    {
                        Title = note.Title,
                        Description = note.Description,
                        CreatedAt=DateTime.Now
                    }
                };
            }
            catch(Exception ex) 
            {
                throw new Exception("unable to create notes",ex);
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
                    StatusCode=(int) HttpStatusCode.OK,
                    Success = true,
                    Message = $"note with { title } is deleted",
                    Data = new NoteDTO()
                    {
                        NoteId=notes.NoteId,
                        Title = notes.Title,
                        CreatedAt=notes.CreatedAt,
                        Description=notes.Description,
                        UserId = notes.UserId
                    }
                };
            }
            catch(Exception ex) 
            {
                throw new Exception($"note with {title} is not deleted");
            }
        }

        public IEnumerable<Note> GetNotes(int userId)
        {
            return _context.Notes.ToList();
        }

        public ResponseModel<NoteDTO> UpdateNote(CreateNoteDTO createNote)
        {
            var notes = _context.Notes.FirstOrDefault(note=>note.Title.Equals(createNote.Title));
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
                            NoteId = notes.NoteId,
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
                    throw new Exception("updation unsuccessfull");
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
    }
}
