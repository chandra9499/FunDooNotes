using Model.Models.DTOs.Labels;
using Model.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface ILabelBLL
    {
        public IEnumerable<Labels> GetAllLabelsForTheNote(int noteId);
    }
}