using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using GotExplorer.DAL.Models;
using System.Data;
using System.Reflection.Emit;

namespace GotExplorer.DAL
{
    public class AppDbContext : IdentityDbContext<User,UserRole,int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
