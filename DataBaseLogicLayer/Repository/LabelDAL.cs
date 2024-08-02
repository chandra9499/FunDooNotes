using DataBaseLogicLayer.Context;
using DataBaseLogicLayer.Interface;
using Microsoft.EntityFrameworkCore;
using Model.Models.DTOs.Labels;
using Model.Models.Entity;
using Model.Models.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLayer.Repository
{
    public class LabelDAL : ILabelDAL
    {
        private readonly FunDooDataBaseContext _context;
        public LabelDAL(FunDooDataBaseContext context) 
        {
            _context = context;
        }
        public IEnumerable<Labels> GetAllLabel()
        {
            return _context.Labels.Include(note => note.Notes).ToList();
        }
    }
}
