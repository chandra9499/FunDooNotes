using BusinessLogicLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Models.DTOs.Note;
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
        public NoteController(INoteBLL noteBLL)
        {
            _noteBLL = noteBLL;
        }

        [HttpPost]
        public IActionResult CreateNote(CreateNoteDTO createNote)
        {
            if(!ModelState.IsValid) 
            {
                return BadRequest(new ErrorStatus() { StatusCode=(int) HttpStatusCode.NotAcceptable,Message="pass all the requiered fields" });
            }
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value;
            var status = _noteBLL.CreateNote(Convert.ToInt32(userId), createNote);
            return Ok(status);
        }
        [HttpPut]
        public IActionResult UpdateNote([FromQuery]CreateNoteDTO createNote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorStatus() { StatusCode = (int)HttpStatusCode.NotAcceptable, Message = "pass all the requiered fields" });
            }
            var status = _noteBLL.UpdateNote(createNote);
            return Ok(status);
        }
        [HttpDelete]
        public IActionResult DeleteNote([FromQuery]string title)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorStatus() { StatusCode = (int)HttpStatusCode.NotAcceptable, Message = "pass all the requiered fields" });
            }
            var status = _noteBLL.DeleteNote(title);
            return Ok(status);  
        }
        [HttpGet]
        public IActionResult GetNotes()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorStatus() { StatusCode = (int)HttpStatusCode.NotAcceptable, Message = "pass all the requiered fields" });
            }
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value;
            var status = _noteBLL.GetNotes(Convert.ToInt32(userId));
            return Ok(status);
        }
        [HttpGet]
        public IActionResult GetNoteByTitle([FromQuery]string title)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(new ErrorStatus() {StatusCode = (int) HttpStatusCode.BadRequest,Message= "pass all the requiered fields" });
            }
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value;
            var status = _noteBLL.GetNoteByTitle(Convert.ToInt32(userId),title);
            return Ok(status);
        }
        [HttpPut]
        public IActionResult AddColourToNote([FromQuery]UpdateColourModel updateColour)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(new ErrorStatus() { StatusCode = (int)HttpStatusCode.BadRequest, Message = "pass all the requiered fields" });
            }
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value;
            var status = _noteBLL.AddColourToNote(Convert.ToInt32(userId),updateColour);
            return Ok(status);
        }
    }
}
