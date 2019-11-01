using AutoMapper;
using Bizico_Project;
using Database.Models;
using Dtos;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;

namespace Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projects;
        private readonly IMapper _mapper;
        private readonly ProjectValidator _validator;
        public ProjectController(IProjectService projects, IMapper mapper, ProjectValidator validator)
        {
            _projects = projects;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpPost("Create")]
        public IActionResult CreateProject(ProjectDto project)
        {
            var val = _validator.Validate(project, ruleSet: "Create");
            if (!val.IsValid)
            {
                return BadRequest(val.Errors);
            }
            var prj = _mapper.Map<Project>(project);
            return Created("", _projects.CreateProject(prj));
        }

        [HttpGet("Read")]
        public IActionResult GetAll([FromQuery]PagingParameterModel paging)
        {
            var projects = _projects.GetProjects(paging.pageNumber, paging.pageSize, paging.sorting);
            return Ok(_mapper.Map<IEnumerable<ProjectDto>>(projects));
        }

        [HttpGet("ReadByUserId/{id}")]
        public IActionResult GetByUserId(Guid id)
        {
            var projects = _projects.GetProjectByUserId(id);
            return Ok(_mapper.Map<IEnumerable<ProjectDto>>(projects));
        }

        [HttpGet("ReadById/{id}")]
        public IActionResult GetById(Guid id)
        {
            var projects = _projects.GetProjectById(id);
            return Ok(_mapper.Map<ProjectDto>(projects));
        }

        [HttpGet("ReadMyProjects")]
        public IActionResult GetMyProjects()
        {
            var projects = _projects.GetMyProjects();
            return Ok(_mapper.Map<IEnumerable<ProjectDto>>(projects));
        }

        [Authorize(Roles = "Client")]
        [HttpPatch("Update")]
        public IActionResult UpdateProject(ProjectDto project)
        {
            var val = _validator.Validate(project, ruleSet: "Update");
            if (!val.IsValid)
            {
                return BadRequest(val.Errors);
            }
            var prj = _mapper.Map<Project>(project);
            return StatusCode((int)HttpStatusCode.NoContent, _projects.UpdateProject(prj));
        }

        [Authorize(Roles = "Moderator, Client")]
        [HttpDelete("Delete/{id}")]
        public IActionResult DeletePrj(Guid id)
        {
            _projects.Delete(id);
            return StatusCode((int)HttpStatusCode.NoContent);
        }
    }
}
