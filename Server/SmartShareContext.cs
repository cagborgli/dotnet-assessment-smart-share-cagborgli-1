using Microsoft.EntityFrameworkCore;

namespace Server
{
    public class SmartShareContext : DbContext
    {
        public DbSet<File> Files { get; set; }

        //Unique specification for Filename 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<File>()
                .HasAlternateKey(t => t.FileName)
                .HasName("AlternateKey_FileName");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("server=127.0.0.1;port=5432;database=smartshare;userid=postgres;password=bondstone");
        }
    }
}