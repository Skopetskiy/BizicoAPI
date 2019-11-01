using Database;
using Database.Models;
using Services.Interfaces;
using System;
using System.Linq;

namespace Services
{
    public class ProfileService : IProfileService
    {
        private readonly CurrentUserInfo _info;
        private readonly FreelanceContext _profilecontext;

        public ProfileService(FreelanceContext profilecontext, CurrentUserInfo info)
        {
            _profilecontext = profilecontext;
            _info = info;
        }

        public IQueryable<Profile> GetAll()
        {
            return _profilecontext.Profiles;
        }

        public IQueryable<Profile> GetProfiles(int page, int size, string sorting)
        {
            var allProfiles = _profilecontext.Profiles;
            var data = allProfiles.AsQueryable();
            data = SortService.ApplySort(data, sorting);

            return data.Skip(size * (page - 1)).Take(size);
        }

        public Profile GetProfileById(Guid id)
        {
            var profile = GetAll().Single(x => x.Id == id);
            return profile;
        }

        public Profile GetMyInfo()
        {
            var id = _info.Id;
            var profile = _profilecontext.Profiles.First(x => x.UserId.ToString() == id);
            return profile;
        }

        public Profile GetProfileByUserId(Guid id)
        {
            var profile = GetAll().Single(x => x.UserId == id);
            return profile;
        }

        public Profile CreateProfile(Profile profile)
        {
            _profilecontext.Profiles.Add(profile);
            _profilecontext.SaveChanges();

            return profile;
        }

        public Profile UpdateProfile(Profile profile)
        {
            var id = _profilecontext.Profiles.First(x => x.UserId.ToString() == _info.Id).Id;
            profile.UserId = Guid.Parse(_info.Id);
            profile.Id = id;
            _profilecontext.ChangeTracker.Entries().ElementAt(0).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            _profilecontext.SaveChanges();
            _profilecontext.Attach(profile);
            _profilecontext.Entry(profile).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _profilecontext.SaveChanges();

            return profile;
        }

        public void Delete(Guid Id)
        {
            _profilecontext.Remove(GetProfileById(Id));
            _profilecontext.SaveChanges();
        }

       
        public Profile ResetFields(string columns, Guid id)
        {
            var user = GetProfileByUserId(id);
            var cols = columns.Split(',');

            foreach (var x in cols)
            {
                switch (x)
                {
                    case "FirstName": user.FirstName = ""; break;
                    case "LastName": user.LastName = ""; break;
                    case "Summary": user.Summary = ""; break;
                    case "Experience": user.Experience = 0; break;
                    default: break;
                }
            }

            _profilecontext.SaveChanges();
            return user;
        }
    }
}
