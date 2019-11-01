using AutoMapper;
using Database.Models;
using Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Controllers
{
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public UserController(IUserService userservice, IMapper mapper, UserManager<User> userManager)
        {
            _userService = userservice;
            _mapper = mapper;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpGet("Read")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(_mapper.Map<IEnumerable<UserDto>>(users));
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpGet("FindByLogin")]
        public IActionResult FindByLogin(string login)
        {
            return Ok(_mapper.Map<UserDto>(_userService.GetByLogin(login)));
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpGet("FindById")]
        public IActionResult FindById(Guid Id)
        {
            return Ok(_mapper.Map<UserDto>(_userService.GetById(Id)));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("CreateModerator")]
        public async Task<IActionResult> CreateModerator(Guid id)
        {
            var user = _userService.GetById(id);
            await _userManager.RemoveFromRoleAsync(user, "client");
            await _userManager.AddToRoleAsync(user, "moderator");

            return Ok("Роль указанного пользователя успешно изменена.");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("DeleteModerator")]
        public async Task<IActionResult> RemoveModerator(Guid id)
        {
            var user = _userService.GetById(id);
            await _userManager.RemoveFromRoleAsync(user, "moderator");
            await _userManager.AddToRoleAsync(user, "client");

            return Ok("Роль указанного пользователя успешно изменена.");
        }

        [Authorize(Roles = "Moderator, Admin")]
        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteAccount(Guid id)
        {
            _userService.Delete(id);
            return StatusCode((int)HttpStatusCode.NoContent);
        }

        [Authorize(Roles = "Client")]
        [HttpDelete("Delete")]
        public IActionResult DeleteMyAccount()
        {
            _userService.DeleteMyAccount();
            return StatusCode((int)HttpStatusCode.NoContent);
        }
    }
}
