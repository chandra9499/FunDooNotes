using BusinessLogicLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Models.DTOs.User;
using Model.Models.Utility;
using System.Net;


namespace FunDooNotes.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBLL _userBLL;
        public UserController(IUserBLL userBLL) 
        {
            _userBLL = userBLL;
        }
        [HttpPost]
        public IActionResult RegisterUser([FromBody] RegisterUserModel registerUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorStatus{ StatusCode = 0, Message = "Please pass all the fields" });
            }
            var status = _userBLL.RegisterUser(registerUser);

            if (!status.Success)
            {
                return Conflict(status);
            }
            return Ok(status);
        }
        [HttpPost]
        public IActionResult UserLogin(LoginModel loginModel) { 
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorStatus { StatusCode = 0, Message = "Please pass all the fields" });
            }
            var status = _userBLL.LoginUser(loginModel);
            return Ok(status);
        }
        [HttpGet]
        [Authorize(Policy = "Userid")]
        public IActionResult GetProfile()
        {
            
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value;
            Console.WriteLine(userId);
            //var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var status = _userBLL.GetProfile(Convert.ToInt32(userId));
            return Ok(status);
        }
        [HttpPut]
        [Authorize(Policy = "Userid")]
        public IActionResult UpdateUserEmail([FromQuery]UpdateUserEmailModel updateUser)
        { 
            if (!ModelState.IsValid) 
            {
                return BadRequest(new ErrorStatus { StatusCode = (int) HttpStatusCode.BadRequest,Message="plese pass all the requiered parameter" });
            }
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value;
            var status = _userBLL.UpdateUserEmail(Convert.ToInt32(userId), updateUser);
            return Ok(status);
        }
    }
}
