using Database.Models;
using System;
using System.Linq;

namespace Services.Interfaces
{
    public interface IProfileService
    {
        IQueryable<Profile> GetProfiles(int page, int size, string sorting);
        Profile GetProfileById(Guid id);
        Profile CreateProfile(Profile profile);
        Profile GetProfileByUserId(Guid id);
        Profile UpdateProfile(Profile profile);
        void Delete(Guid id);
        Profile GetMyInfo();
        Profile ResetFields(string columns, Guid id);
    }
}
