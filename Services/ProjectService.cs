using Bizico_Project;
using Database;
using Database.Models;
using System;
using System.Linq;
using System.Linq.Dynamic;

namespace Services
{
    public class ProjectService : IProjectService
    {
        private readonly FreelanceContext _projectcontext;
        private readonly CurrentUserInfo _info;

        public ProjectService(FreelanceContext projectcontext, CurrentUserInfo info)
        {
            _projectcontext = projectcontext;
            _info = info;
        }

        public IQueryable<Project> GetProjects()
        {
            return _projectcontext.Projects;
        }

        public IQueryable<Project> GetProjects(int page, int size, string sorting)
        {
            var allProjects = _projectcontext.Projects;
            var data = allProjects.AsQueryable();
            data = SortService.ApplySort(data, sorting);

            return data.Skip(size*(page-1)).Take(size);
        }


        public Project GetProjectById(Guid id)
        {
            var project = GetProjects().Single(x => x.Id == id);
            return project;
        }

        public IQueryable<Project> GetProjectByUserId(Guid id)
        {
            var Project = GetProjects().Where(x => x.UserId == id);
            return Project;
        }

        public IQueryable<Project> GetMyProjects()
        {
            var Project = GetProjects().Where(x => x.UserId.ToString() == _info.Id);
            return Project;
        }

        public Project CreateProject(Project project)
        {
            project.UserId = Guid.Parse(_info.Id);
            project.Date = DateTime.Now;
            _projectcontext.Projects.Add(project);
            _projectcontext.SaveChanges();

            return project;
        }       

        public Project UpdateProject(Project project)
        {
            project.UserId = Guid.Parse(_info.Id);
            var userid = _projectcontext.Projects.First(x => x.Id == project.Id).UserId;
            if (userid.ToString() == _info.Id)
            {
                var lst = _projectcontext.ChangeTracker.Entries().ElementAt(0).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                _projectcontext.Attach(project);
                _projectcontext.Entry(project).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _projectcontext.SaveChanges();
            }

            return project;
        }

        public void Delete(Guid Id)
        {
            if (_info.Role == "Client")
            {
                var userid = _projectcontext.Projects.First(x => x.Id == Id).UserId;
                if (userid.ToString() == _info.Id)
                {
                    _projectcontext.Remove(GetProjectById(Id));
                    _projectcontext.SaveChanges();
                }
            }
            else
            {
                _projectcontext.Remove(GetProjectById(Id));
                _projectcontext.SaveChanges();
            }
        }
    }
}
