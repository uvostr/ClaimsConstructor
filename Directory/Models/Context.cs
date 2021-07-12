using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Directory.Models
{
    public class Context : DbContext
    {
        public DbSet<Lawsuit> Lawsuits { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<AdditionBlocks> AdditionBlocks { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Test> Tests { get; set; }
    }
}