using AutoMapper;
using Database.Models;
using Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;

namespace Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profiles;
        private readonly IMapper _mapper;
        public ProfileController(IProfileService profiles, IMapper mapper)
        {
            _profiles = profiles;
            _mapper = mapper;
        }

        [HttpGet("Read")]
        public IActionResult GetAll([FromQuery]PagingParameterModel paging)
        {
            var profiles = _profiles.GetProfiles(paging.pageNumber, paging.pageSize, paging.sorting);
            return Ok(_mapper.Map<IEnumerable<ProfileDto>>(profiles));
        }

        [HttpGet("ReadByUserId/{id}")]
        public IActionResult GetByUserId(Guid id)
        {
            var profiles = _profiles.GetProfileByUserId(id);
            return Ok(_mapper.Map<ProfileDto>(profiles));
        }

        [HttpGet("ReadById/{id}")]
        public IActionResult GetById(Guid id)
        {
            var profiles = _profiles.GetProfileById(id);
            return Ok(_mapper.Map<ProfileDto>(profiles));
        }


        [Authorize(Roles = "Client")]
        [HttpGet("ReadMyInfo")]
        public IActionResult GetMyInfo()
        {
            var profile = _profiles.GetMyInfo();
            return Ok(_mapper.Map<ProfileDto>(profile));
        }

        [Authorize(Roles = "Client")]
        [HttpPatch("Update")]
        public IActionResult UpdateProfiles(ProfileDto profile)
        {
            var prof = _mapper.Map<Database.Models.Profile>(profile);
            return StatusCode((int)HttpStatusCode.NoContent, _profiles.UpdateProfile(prof));
        }

        [Authorize(Roles = "Moderator")]
        [HttpPatch("ResetField")]
        public IActionResult ResetField(string columns, Guid id)
        {
            return StatusCode((int)HttpStatusCode.NoContent, _profiles.ResetFields(columns, id));
        }
    }
}
