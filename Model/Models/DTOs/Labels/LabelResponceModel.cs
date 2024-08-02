
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models.Entity;

namespace Model.Models.DTOs.Labels
{
    public class LabelResponceModel
    {
        public int LabelsId { get; set; }
        public string? LabelsName { get; set; }
        public string? LabelsDescription { get; set; }
        public ICollection<Model.Models.Entity.Note>? Notes { get; set; }
    }
}
