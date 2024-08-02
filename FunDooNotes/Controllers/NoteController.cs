using BusinessLayer.Interface;
using BusinessLogicLayer.Interface;
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
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteBLL _noteBLL;
        private readonly ICollaboratorBLL _collaboratorBLL;
        private readonly ILabelBLL _labelBLL;
        public NoteController(INoteBLL noteBLL, ICollaboratorBLL collaboratorBLL, ILabelBLL labelBLL)
        {
            _noteBLL = noteBLL;
            _collaboratorBLL = collaboratorBLL;
            _labelBLL = labelBLL;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote(CreateNoteDTO createNote)
        {
            if(!ModelState.IsValid) 
            {
                return BadRequest(new ErrorStatus() { StatusCode=(int) HttpStatusCode.NotAcceptable,Message="pass all the requiered fields" });
            }
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value;
            var status = await _noteBLL.CreateNoteAsync(Convert.ToInt32(userId), createNote);
            return Ok(status);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateNote([FromQuery]CreateNoteDTO createNote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorStatus() { StatusCode = (int)HttpStatusCode.NotAcceptable, Message = "pass all the requiered fields" });
            }
            var status = await _noteBLL.UpdateNoteAsync(createNote);
            return Ok(status);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteNote([FromQuery]string title)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorStatus() { StatusCode = (int)HttpStatusCode.NotAcceptable, Message = "pass all the requiered fields" });
            }
            var status = await _noteBLL.DeleteNoteAsync(title);
            return Ok(status);  
        }
        [HttpGet]
        public async Task<IActionResult> GetNotes()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorStatus() { StatusCode = (int)HttpStatusCode.NotAcceptable, Message = "pass all the requiered fields" });
            }
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value;
            var status = await _noteBLL.GetNotesAsync(Convert.ToInt32(userId));
            return Ok(status);
        }
        [HttpGet]
        public async Task<IActionResult> GetNoteById([FromQuery]int noteId)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(new ErrorStatus() {StatusCode = (int) HttpStatusCode.BadRequest,Message= "pass all the requiered fields" });
            }
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value;
            var status = await _noteBLL.GetNoteByIdAsync(Convert.ToInt32(userId), noteId);
            return Ok(status);
        }
        [HttpPut]
        public async Task<IActionResult> AddColourToNote([FromQuery]UpdateColourModel updateColour)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(new ErrorStatus() { StatusCode = (int)HttpStatusCode.BadRequest, Message = "pass all the requiered fields" });
            }
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value;
            var status = await _noteBLL.AddColourToNoteAsync(Convert.ToInt32(userId),updateColour);
            return Ok(status);
        }
        [HttpPost]
        public async Task<IActionResult> AddLabelsToNote([FromQuery]int noteId,[FromQuery] LabelRequestModel addLabels)
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
        [HttpPost]
        public IActionResult AddCollaborator([FromQuery]int noteId, [FromQuery]CollaboratorModel collaboratorModel)
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

        [HttpGet]
        public IActionResult GetAllLabels()
        {
            var status = _labelBLL.GetAllLabel();
            return Ok(status);
        }
    }
}
