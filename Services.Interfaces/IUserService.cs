using Database.Models;
using System;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IUserService
    {
        IEnumerable<User> GetAll();
        User GetById(Guid id);
        User GetByLogin(string login);
        void Delete(Guid Id);
        void DeleteMyAccount();
    }
}
