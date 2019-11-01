using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Database.Models
{
    public class User : IdentityUser<Guid>
    {
        public string Password { get; set; }
        public string Token { get; set; }
        public Profile Profile { get; set; }
        public List<Request> Requests { get; set; }
        public List<Project> Projects { get; set; }
        public List<UserRole> UserRoles { get; set; }
        public User()
        {
            Requests = new List<Request>();
            Projects = new List<Project>();
            UserRoles = new List<UserRole>();
        }
    }
}
