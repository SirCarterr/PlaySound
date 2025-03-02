using Microsoft.EntityFrameworkCore;
using PlaySound.Model;
using System;
using System.IO;

namespace PlaySound.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Audio> Audios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=" + Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PlaySound.db"));
        }
    }
}
