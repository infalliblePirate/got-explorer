using GotExplorer.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace GotExplorer.DAL.Entities
{
    public class UserRole : IdentityRole<int>, IEntity<int>
    {
        public UserRole() : base() { }
        public UserRole(string roleName) : base(roleName) { }
    }
}
