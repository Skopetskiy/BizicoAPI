using Database;
using Database.Models;
using Microsoft.AspNetCore.Identity;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly FreelanceContext _context;
        private readonly UserManager<User> _userManager;
        private readonly CurrentUserInfo _info;

        public UserService(FreelanceContext context, UserManager<User> userManager, CurrentUserInfo info)
        {
            _context = context;
            _userManager = userManager;
            _info = info;
        }

        public IEnumerable<User> GetAll()
        {
            var users = _context.Users;

            return users;
        }

        public User GetById(Guid id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            return user;
        }

        public User GetByLogin(string login)
        {
            var user = _context.Users.FirstOrDefault(x => x.UserName == login);
            return user;
        }

        public void Delete(Guid Id)
        {
            var id = _context.UserRoles.First(x => x.UserId == Id).RoleId;
            var role = _context.Roles.First(x => x.Id == id);

            if (role.Name == "Client")
            {
                _context.Remove(GetById(Id));
                _context.SaveChanges();
            }
        }

        public void DeleteMyAccount()
        {
            var id = _info.Id;
            _context.Remove(GetById(Guid.Parse(id)));
            _context.SaveChanges();
        }
    }
}
