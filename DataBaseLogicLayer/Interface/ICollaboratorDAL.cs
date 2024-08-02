using Model.Models.DTOs.Collaborator;
using Model.Models.Entity;
using Model.Models.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLogicLayer.Interface
{
    public interface ICollaboratorDAL
    {
        ResponseModel<CollaboratorModel> AddCollaborator(int noteId, CollaboratorModel collaboratorModel);
        ResponseModel<Collaborator> RemoveCollaborator(int noteId, CollaboratorModel collaboratorModel);
        ResponseModel<List<Collaborator>> GetCollaborators(int noteId);
    }
}
