using DataBaseLogicLayer.Context;
using DataBaseLogicLayer.Interface;
using Microsoft.EntityFrameworkCore;
using Model.Models.DTOs.Collaborator;
using Model.Models.Entity;
using Model.Models.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DataBaseLogicLayer.Repository
{
    public class CollaboratorDAL : ICollaboratorDAL
    {
        private readonly FunDooDataBaseContext _context;

        public CollaboratorDAL(FunDooDataBaseContext context)
        {
            _context = context;
        }

        public ResponseModel<CollaboratorModel> AddCollaborator(int noteId, CollaboratorModel collaboratorModel)
        {
            try
            {
                var collaboratorUser = _context.Users.FirstOrDefault(user => user.Email.Equals(collaboratorModel.CollaboratorEmail));
                if (collaboratorUser == null)
                {
                    return new ResponseModel<CollaboratorModel>()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Success = false,
                        Message = $"There is no user with email {collaboratorModel.CollaboratorEmail}",
                        Data = null
                    };
                }
                var note = _context.Notes.FirstOrDefault(note => note.NoteId.Equals(noteId));
                if (note == null)
                {
                    return new ResponseModel<CollaboratorModel>()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Success = false,
                        Message = $"The note {noteId} does not exist",
                        Data = null
                    };
                }
                var existingCollborator = _context.Collaborators.Where(collb=>collb.CollaboratorEmail.Equals(collaboratorModel.CollaboratorEmail) && collb.NoteId.Equals(noteId)).FirstOrDefault();
                if (existingCollborator != null)
                {
                    return new ResponseModel<CollaboratorModel>()
                    {
                        StatusCode = (int)HttpStatusCode.Conflict,
                        Success = false,
                        Message = $"There is allready user with email {collaboratorModel.CollaboratorEmail}",
                        Data = null
                    };
                }
                var collaborator = new Collaborator()
                {
                    NoteId = noteId,
                    CollaboratorUserId = collaboratorUser.UserId,
                    CollaboratorEmail = collaboratorUser.Email,
                };
                _context.Collaborators.Add(collaborator);
                _context.SaveChanges();
                return new ResponseModel<CollaboratorModel>()
                {
                    StatusCode = (int)HttpStatusCode.Created,
                    Success = true,
                    Message = $"Added collaborator {collaboratorModel.CollaboratorEmail} to the note {noteId}",
                    Data = collaboratorModel                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<CollaboratorModel>()
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Success = false,
                    Message = $"An error occurred while adding the collaborator: {ex.Message}",
                    Data = null
                };
            }
        }

        public ResponseModel<List<Collaborator>> GetCollaborators(int noteId)
        {
            try
            {
                var collaborators = _context.Collaborators.Where(collb => collb.NoteId.Equals(noteId)).ToList();
                if (!collaborators.Any())
                {
                    return new ResponseModel<List<Collaborator>>()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Success = false,
                        Message = $"No collaborators found for the note {noteId}",
                        Data = null
                    };
                }

                return new ResponseModel<List<Collaborator>>()
                {
                    StatusCode = (int)HttpStatusCode.Found,
                    Success = true,
                    Message = $"All the collaborators of the note {noteId}",
                    Data = collaborators
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<Collaborator>>()
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Success = false,
                    Message = $"An error occurred while retrieving the collaborators: {ex.Message}",
                    Data = null
                };
            }
        }

        public ResponseModel<Collaborator> RemoveCollaborator(int noteId, CollaboratorModel collaboratorModel)
        {
            try
            {
                var collaborator = _context.Collaborators.FirstOrDefault(collb => collb.NoteId.Equals(noteId) && collb.CollaboratorEmail.Equals(collaboratorModel.CollaboratorEmail));
                if (collaborator == null)
                {
                    return new ResponseModel<Collaborator>()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Success = false,
                        Message = $"No collaborator found with email {collaboratorModel.CollaboratorEmail} for the note {noteId}",
                        Data = null
                    };
                }

                _context.Collaborators.Remove(collaborator);
                _context.SaveChanges();

                return new ResponseModel<Collaborator>()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Success = true,
                    Message = $"Removed collaborator {collaboratorModel.CollaboratorEmail} from the note {noteId}",
                    Data = collaborator
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<Collaborator>()
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Success = false,
                    Message = $"An error occurred while removing the collaborator: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}
