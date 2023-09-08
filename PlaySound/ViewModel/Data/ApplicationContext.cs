using Microsoft.EntityFrameworkCore;
using PlaySound.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaySound.ViewModel.Data
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
