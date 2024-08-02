using BusinessLogicLayer.Interface;
using DataBaseLogicLayer.Interface;
using Model.Models.DTOs.Collaborator;
using Model.Models.Entity;
using Model.Models.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Service
{
    public class CollaboratorBLL : ICollaboratorBLL
    {
        private readonly ICollaboratorDAL _collaboratorDAL;
        public CollaboratorBLL(ICollaboratorDAL collaboratorDAL)
        {
            _collaboratorDAL = collaboratorDAL;
        }
        public ResponseModel<CollaboratorModel> AddCollaborator(int noteId, CollaboratorModel collaboratorModel)
        {
            return _collaboratorDAL.AddCollaborator(noteId, collaboratorModel);
        }

        public ResponseModel<List<Collaborator>> GetCollaborators(int noteId)
        {
            return _collaboratorDAL.GetCollaborators(noteId);
        }

        public ResponseModel<Collaborator> RemoveCollaborator(int noteId, CollaboratorModel collaboratorModel)
        {
            return _collaboratorDAL.RemoveCollaborator(noteId, collaboratorModel);
        }
    }
}
