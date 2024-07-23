using Model.Models.DTOs.User;
using Model.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLogicLayer.Helper
{
    public class BCryptPassword
    {
        public static string BCryptThePassWord(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        public static bool VarifyThePassword(string enteredPassword,string userPassword)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, userPassword); ;
        }
    }
}
