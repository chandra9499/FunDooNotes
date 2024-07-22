using BusinessLogicLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Models.DTOs.User;
using Model.Models.Utility;

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
    }
}
