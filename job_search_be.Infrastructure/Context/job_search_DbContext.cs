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
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Refresh_Token> RefreshTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("Users");
                e.HasKey(e => e.UserId);
                e.HasOne(e => e.Role).WithMany(e=> e.Users).HasForeignKey(e=>e.RoleId).OnDelete(DeleteBehavior.ClientSetNull);
            });
            modelBuilder.Entity<Role>(e =>
            {
                e.ToTable("Roles");
                e.HasKey(e => e.RoleId);
            });
            modelBuilder.Entity<Refresh_Token>(e =>
            {
                e.ToTable("RefreshTokens");
                e.HasKey(e => e.Refresh_TokenId);
                e.HasOne(e => e.User).WithOne(e => e.RefreshToken).HasForeignKey<User>(e => e.UserId);
            });
        }
    }
}
