using Model.Models.DTOs.Labels;
using Model.Models.Entity;
using Model.Models.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLogicLayer.Interface
{
    public interface ILabelDAL
    {
        public IEnumerable<Labels> GetAllLabel();
    }
}
