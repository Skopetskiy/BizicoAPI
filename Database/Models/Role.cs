using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Database.Models
{
    public class Role : IdentityRole<Guid>
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public List<UserRole> UserRoles { get; set; }
        public Role()
        {
            UserRoles = new List<UserRole>();
        }
    }
}
