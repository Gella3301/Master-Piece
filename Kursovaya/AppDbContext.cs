using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kursovaya.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kursovaya
{
    public class AppDbContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet <Genre> Genres { get; set; }
        public DbSet <Piece> Pieces { get; set; }
        public DbSet <Session> Sessions { get; set; }
        public DbSet<Achievement> Achievements { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = @"Server=(localdb)\MSSQLLocalDB; Database=MasterPiece; Integrated
        Security=True;";
        }
       //protected
    }
}
