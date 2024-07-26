using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models.Entity
{
    public class Labels
    {
        [Key]
        public int LabelsId { get; set; }
        public string? LabelsName { get; set; }
        public string? LabelsDescription { get; set;}        
        public List<Note>? Notes  { get; set; }
    }
}
