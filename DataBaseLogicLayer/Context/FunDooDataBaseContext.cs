using Microsoft.EntityFrameworkCore;
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
        public FunDooDataBaseContext(DbContextOptions<FunDooDataBaseContext> options) : base(options)
        {
        }
        public DbSet<Collaborator> Collaborators { get; set; }
        public DbSet<Labels> Labels { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //many Many relation between notes and lables
            modelBuilder.Entity<Note>()
                .HasMany(x => x.Labels)
                .WithMany(x => x.Notes)
                .UsingEntity(j => j.ToTable("NoteLabels"));
            //mane-many relation between User and Note
            modelBuilder.Entity<User>()
                .HasMany(x => x.Notes)
                .WithOne(y => y.User)
                .OnDelete(DeleteBehavior.Cascade);

            //one- many relation between notes and collaburators
            modelBuilder.Entity<Collaborator>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.CollaboratorUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Collaborator>()
                .HasOne(x => x.Note)
                .WithMany(x => x.Collaborators)
                .HasForeignKey(x => x.NoteId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
