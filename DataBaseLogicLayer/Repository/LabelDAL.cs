using DataBaseLayer.Interface;
using DataBaseLogicLayer.Context;
using DataBaseLogicLayer.Interface;
using Microsoft.EntityFrameworkCore;
using Model.Models.DTOs.Labels;
using Model.Models.Entity;
using Model.Models.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseLayer.Repository
{
    public class LabelDAL : ILabelDAL
    {
        private readonly FunDooDataBaseContext _context;
        private readonly ICacheDL _cacheDL;

        public LabelDAL(FunDooDataBaseContext context, ICacheDL cacheDL)
        {
            _context = context;
            _cacheDL = cacheDL;
        }

        public IEnumerable<Labels> GetAllLabelsForTheNote(int noteId)
        {
            var cacheKey = $"note:{noteId}_allLabels";
            var labels = _context.Labels.Include(note => note.Notes).ToList();
            if (labels.Any())
            {
                _cacheDL.SetData(cacheKey, labels, DateTimeOffset.Now.AddMinutes(20));
            }

            return labels;
        }
    }
}
