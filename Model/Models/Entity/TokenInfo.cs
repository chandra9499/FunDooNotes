﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models.Entity
{
    public class TokenInfo
    {
        [Key]
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
        public DateTime TokenExpiry { get; set; }
    }
}
