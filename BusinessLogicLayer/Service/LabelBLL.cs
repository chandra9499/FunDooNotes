using BusinessLayer.Interface;
using DataBaseLogicLayer.Interface;
using Model.Models.DTOs.Labels;
using Model.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class LabelBLL : ILabelBLL
    {
        private readonly ILabelDAL _labelDAL;
        public LabelBLL(ILabelDAL labelDAL)
        {
            _labelDAL = labelDAL;
        }

        public IEnumerable<Labels> GetAllLabel()
        {
            return _labelDAL.GetAllLabel();
        }
    }
}
