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
    }
}