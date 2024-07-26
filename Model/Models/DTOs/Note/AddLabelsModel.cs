using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models.DTOs.Note
{
    public class AddLabelsModel
    {
        public string? Title { get; set; }
        public List<string>? Labels { get; set;}
    }
}
