using Database.Models;
using System;
using System.Linq;

namespace Bizico_Project
{
    public interface IProjectService
    {
        IQueryable<Project> GetProjects();
        IQueryable<Project> GetProjects(int page, int size, string sorting);
        Project GetProjectById(Guid id);
        Project CreateProject(Project project);
        IQueryable<Project> GetMyProjects();
        IQueryable<Project> GetProjectByUserId(Guid id);
        Project UpdateProject(Project project);
        void Delete(Guid id);
    }
}