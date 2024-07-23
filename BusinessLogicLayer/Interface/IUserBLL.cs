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
        ResponseModel<UpdateUserEmailModel> UpdateUserEmail(int userId, UpdateUserEmailModel updateUser);
        ResponseModel<string> ChangePassword(int userId, ChangePasswordModel passwordModel);
        bool ResetPassword(string email);
    }
}
