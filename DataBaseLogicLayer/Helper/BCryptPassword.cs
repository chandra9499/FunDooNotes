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
        public string VarifyThePassword(string password)
        {
            return null;
        }
    }
}
