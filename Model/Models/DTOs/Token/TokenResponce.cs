using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models.DTOs.Token
{
    public class TokenResponce
    {
        public string? TokenString { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
