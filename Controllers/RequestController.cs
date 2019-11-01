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
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requests;
        private readonly IMapper _mapper;
        private readonly RequestValidator _validator;
        public RequestController(IRequestService requests, IMapper mapper, RequestValidator validator)
        {
            _requests = requests;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpPost("Create")]
        public IActionResult CreateRequest(RequestDto request)
        {
            var val = _validator.Validate(request, ruleSet:"Create");
            if (!val.IsValid)
            {
                return BadRequest(val.Errors);
            }
            var req = _mapper.Map<Request>(request);
            return Created("", _requests.CreateRequest(req));
        }

        [HttpGet("Read")]
        public IActionResult GetAll([FromQuery]PagingParameterModel paging)
        {
            var requests = _requests.GetRequests(paging.pageNumber, paging.pageSize, paging.sorting);
            return Ok(_mapper.Map<IEnumerable<RequestDto>>(requests));
        }

        [HttpGet("ReadByUserId/{id}")]
        public IActionResult GetByUserId(Guid id)
        {
            var requests = _requests.GetRequestByUserId(id);
            return Ok(_mapper.Map<IEnumerable<RequestDto>>(requests));
        }

        [HttpGet("ReadById/{id}")]
        public IActionResult GetById(Guid id)
        {
            var requests = _requests.GetRequestById(id);
            return Ok(_mapper.Map<RequestDto>(requests));
        }

        [HttpGet("ReadMyRequests")]
        public IActionResult GetMyRequests()
        {
            var requests = _requests.GetMyRequests();
            return Ok(_mapper.Map<IEnumerable<RequestDto>>(requests));
        }

        [RuleSetForClientSideMessages("Update")]
        [Authorize(Roles = "Moderator, Client")]
        [HttpPatch("Update")]
        public IActionResult UpdateRequest(RequestDto request)
        {
            var val = _validator.Validate(request, ruleSet: "Update");
            if (!val.IsValid)
            {
                return BadRequest(val.Errors);
            }
            var req = _mapper.Map<Request>(request);
            return StatusCode((int)HttpStatusCode.NoContent, _requests.UpdateRequest(req));
        }

        [Authorize(Roles = "Moderator, Client")]
        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteReq(Guid id)
        {
            _requests.Delete(id);
            return StatusCode((int)HttpStatusCode.NoContent);
        }
    }
}
