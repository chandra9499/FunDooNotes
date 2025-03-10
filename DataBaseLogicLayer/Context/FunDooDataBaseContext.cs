﻿using Microsoft.EntityFrameworkCore;
using Model.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLogicLayer.Context
{
    public class FunDooDataBaseContext : DbContext
    {
        public FunDooDataBaseContext(DbContextOptions<FunDooDataBaseContext> options) : base(options) { }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<TokenInfo> Tokens { get; set; }

    }
}
