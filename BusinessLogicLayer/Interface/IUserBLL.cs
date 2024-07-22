using Model.Models.DTOs.Token;
using Model.Models.DTOs.User;
using Model.Models.Entity;
using Model.Models.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface
{
    public interface IUserBLL
    {
        ResponseModel<User> RegisterUser(RegisterUserModel registerUser);
        ResponseModel<TokenResponce> LoginUser(LoginModel loginModel);
        ResponseModel<UserResponceModel> GetProfile(int userId);
        void LogoutUser();
        bool UpdateProfile(string userId, User updatedUser);        
        bool ChangePassword(string userId, string oldPassword, string newPassword);
        bool ResetPassword(string email);
    }
}
