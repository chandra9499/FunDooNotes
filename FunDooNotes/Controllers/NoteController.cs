using BusinessLayer.Interface;
using BusinessLogicLayer.Interface;
using DataBaseLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Models.DTOs.Collaborator;
using Model.Models.DTOs.Labels;
using Model.Models.DTOs.Note;
using Model.Models.Entity;
using Model.Models.Utility;
using System.Net;

namespace FunDooNotes.Controllers
{
    [Authorize(Policy = "Userid")]
    [Route("api/note")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteBLL _noteBLL;
        private readonly ICollaboratorBLL _collaboratorBLL;
        private readonly ILabelBLL _labelBLL;
        private readonly ICacheDL _cacheDL; 
        public NoteController(INoteBLL noteBLL, ICollaboratorBLL collaboratorBLL, ILabelBLL labelBLL,ICacheDL cacheDL)
        {
            _noteBLL = noteBLL;
            _collaboratorBLL = collaboratorBLL;
            _labelBLL = labelBLL;
            _cacheDL = cacheDL;
        }

        // POST api/note
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddNote(CreateNoteDTO createNote)
        {
            if(!ModelState.IsValid) 
            {
                return BadRequest(new ErrorStatus() { StatusCode=(int) HttpStatusCode.NotAcceptable,Message="pass all the requiered fields" });
            }
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value;
            var status = await _noteBLL.CreateNoteAsync(Convert.ToInt32(userId), createNote);
            return Ok(status);
        }

        // PUT api/note/{noteId}
        [HttpPut("{noteId}")]
        public async Task<IActionResult> UpdateByNoteId(int noteId,[FromBody]CreateNoteDTO createNote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorStatus() { StatusCode = (int)HttpStatusCode.NotAcceptable, Message = "pass all the requiered fields" });
            }
            var status = await _noteBLL.UpdateNoteAsync(createNote);
            return Ok(status);
        }

        // DELETE api/note/{title}
        [HttpDelete("{noteId}")]
        public async Task<IActionResult> DeleteByNoteId(int noteId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorStatus() { StatusCode = (int)HttpStatusCode.NotAcceptable, Message = "pass all the requiered fields" });
            }
            var status = await _noteBLL.DeleteByNoteIdAsync(noteId);
            return Ok(status);  
        }

        // GET api/note
        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorStatus() { StatusCode = (int)HttpStatusCode.NotAcceptable, Message = "pass all the requiered fields" });
            }
            var userId = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value);
            var cacheKey = $"notes{userId}";
            var cacheData = _cacheDL.GetData<IEnumerable<Note>>(cacheKey);
            if (cacheData != null && cacheData.Any())
            {
                return Ok(cacheData);
            }
            
            var status = await _noteBLL.GetNotesAsync(userId);
            return Ok(status);
        }

        // GET api/note/{noteId}
        [HttpGet("{noteId}")]
        public async Task<IActionResult> GetByNoteId(int noteId)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(new ErrorStatus() {StatusCode = (int) HttpStatusCode.BadRequest,Message= "pass all the requiered fields" });
            }
            var userId = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value);
            var cacheKey = $"notes{userId}";
            var cacheData = _cacheDL.GetData<IEnumerable<Note>>(cacheKey).Where(note=>note.NoteId.Equals(noteId)).FirstOrDefault();
            if (cacheData != null)
            {
                return Ok(cacheData);
            }
            var status = await _noteBLL.GetNoteByIdAsync(Convert.ToInt32(userId), noteId);
            return Ok(status);
        }

        // PUT api/note/{noteId}/color
        [HttpPut("{noteId}/color")]
        public async Task<IActionResult> AssignColourToNote([FromBody]UpdateColourModel updateColour)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(new ErrorStatus() { StatusCode = (int)HttpStatusCode.BadRequest, Message = "pass all the requiered fields" });
            }
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value;
            var status = await _noteBLL.AddColourToNoteAsync(Convert.ToInt32(userId),updateColour);
            return Ok(status);
        }

        // POST api/note/{noteId}/labels
        [HttpPost("{noteId}/labels")]
        public async Task<IActionResult> AssignLabelsToNote(int noteId,[FromBody] LabelRequestModel addLabels)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorStatus() { StatusCode = (int)HttpStatusCode.BadRequest, Message = "pass all the requiered fields" });
            }
            var status = await _noteBLL.AddLabelsToNotesAsync(noteId,addLabels);
            if (status.Success)
            {
                return Conflict(status);
            }
            return Ok(status);
        }

        // POST api/note/{noteId}/collaborators
        [HttpPost("{noteId}/collaborators")]
        public IActionResult AssignCollaboratorToNote(int noteId, [FromBody]CollaboratorModel collaboratorModel)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new ErrorStatus() { StatusCode = (int)HttpStatusCode.BadRequest, Message = "pass all the requiered fields" });
            }
            var status = _collaboratorBLL.AddCollaborator(noteId, collaboratorModel);
            if (status.Success)
            {
                return Conflict(status);
            }            
            return Ok(status);
        }

        // GET api/note/{noteId}/labels
        [HttpGet("{noteId}/labels")]
        public IActionResult GetLabelsForNote(int noteId)
        {
            var cacheKey = $"note:{noteId}_allLabels";
            var cacheData = _cacheDL.GetData<IEnumerable<Labels>>(cacheKey);

            if (cacheData != null && cacheData.Any())
            {
                return Ok(cacheData);
            }

            var status = _labelBLL.GetAllLabelsForTheNote(noteId);
            return Ok(status);
        }

        // PUT api/note/{noteId}/archive
        [HttpPut("{noteId}/archive")]
        public async Task<IActionResult> ArchiveNote(int noteId)
        {
            var status = await _noteBLL.ArchiveNoteAsync(noteId);
            return Ok(status);
        }

        // PUT api/note/{noteId}/unarchive
        [HttpPut("{noteId}/unarchive")]
        public async Task<IActionResult> UnarchiveNote(int noteId)
        {
            var status = await _noteBLL.UnarchiveNoteAsync(noteId);
            return Ok(status);
        }

        // GET api/note/archived
        [HttpGet("archived")]
        public async Task<IActionResult> GetArchivedNotes()
        {
            var userId = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value);
            var status = await _noteBLL.GetArchivedNotesAsync(userId);
            return Ok(status);
        }

    }
}
