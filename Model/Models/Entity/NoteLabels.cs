using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models.Entity
{
    public class NoteLabels
    {
        public virtual Note? Note { get; set; }
        public int NoteId { get; set; }
        public virtual Labels? Labels { get; set; }
        public int LabelId { get; set; } 
    }
}
