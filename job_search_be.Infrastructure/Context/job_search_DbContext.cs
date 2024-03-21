using job_search_be.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Infrastructure.Context
{
    public class job_search_DbContext:DbContext
    {
        public job_search_DbContext(DbContextOptions<job_search_DbContext> options) : base(options) { }
        public virtual DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("Users");
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
