using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kursovaya.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kursovaya
{
    /// <summary>
    /// Класс для подключения к бд
    /// </summary>
    public class AppDbContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet <Genre> Genres { get; set; }
        public DbSet <Piece> Pieces { get; set; }
        public DbSet <Session> Sessions { get; set; }
        public DbSet<Achievement> Achievements { get; set; }

        /// <summary>
        /// Строка подключения к локальной бд
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
          optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB; Database=MasterPiece; Trusted_Connection = True;");
        }

        /// <summary>
        /// отношения таблиц 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
              .HasIndex( u => u.userId)
              .IsUnique();

            modelBuilder.Entity<Genre>()
                .HasIndex(g => g.genreId)
                .IsUnique();

            modelBuilder.Entity<Piece>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.userId);

            modelBuilder.Entity<Piece>()
               .HasOne(p => p.Genre)
               .WithMany()
               .HasForeignKey(p => p.genreId);

            modelBuilder.Entity<Session>()
                .HasIndex(s => s.sessionId)
                .IsUnique();
            modelBuilder.Entity<Achievement>()
                .HasIndex(a => a.achievementId)
                .IsUnique();

        }
    }
}
