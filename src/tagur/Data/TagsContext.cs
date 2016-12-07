using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using tagur.Models;

namespace tagur.Data
{
    public class TagsContext : DbContext
    {
        public TagsContext(DbContextOptions<TagsContext> options) : base(options)
        {
        }

        public DbSet<Uploads> Uploads { get; set; }
        public DbSet<Tags> Tags { get; set; }
        public DbSet<TagsHistory> TagsHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Uploads>().ToTable("Uploads");
            modelBuilder.Entity<Tags>().ToTable("Tags");
            modelBuilder.Entity<TagsHistory>().ToTable("TagsHistory");
        }
    }

}
