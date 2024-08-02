using BusinessLogicLayer.Interface;
using DataBaseLogicLayer.Interface;
using Model.Models.DTOs;
using Model.Models.DTOs.Token;
using Model.Models.DTOs.User;
using Model.Models.Entity;
using Model.Models.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Service
{
    public class UserBLL : IUserBLL
    {
        private readonly IUserDAL _userDAL;
        public UserBLL(IUserDAL userDAL)
        {
            _userDAL = userDAL;
        }
        public ResponseModel<string> ChangePassword(int userId, ChangePasswordModel passwordModel)
        {
            return _userDAL.ChangePassword(userId, passwordModel);
        }

        public ResponseModel<UserResponceModel> GetProfile(int userId)
        {
            return _userDAL.GetProfile(userId);
        }

        public ResponseModel<TokenResponce> LoginUser(LoginModel loginModel)
        {
            return _userDAL.LoginUser(loginModel);
        }

        public void LogoutUser()
        {
            throw new NotImplementedException();
        }

        public ResponseModel<User> RegisterUser(RegisterUserModel registerUser)
        {
            return _userDAL.RegisterUser(registerUser);
        }

        public Task<bool> ResetPassword(EmailModel emailModel)
        {
            return _userDAL.ResetPassword(emailModel);
        }
        public ResponseModel<UpdateUserEmailModel> UpdateUserEmail(int userId, UpdateUserEmailModel updateUser)
        {
            return _userDAL.UpdateUserEmail(userId, updateUser);
        }
        public bool UpdateProfile(string userId, User updatedUser)
        {
            throw new NotImplementedException();
        }
    }
}
