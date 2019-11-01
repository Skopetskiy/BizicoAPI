using Database;
using Database.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly TokenSettings _appSettings;
        private readonly FreelanceContext _context;

        public AccountController(UserManager<User> userManager, IOptions<TokenSettings> applicationSettings, FreelanceContext context)
        {
            _context = context;
            _userManager = userManager;
            _appSettings = applicationSettings.Value;
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> Register(UserRegistration model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Login };
                var result = await _userManager.CreateAsync(user, model.Password);
                var profile = new Profile { UserId = user.Id };
                _context.Profiles.Add(profile);
                _context.SaveChanges();
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
                await _userManager.AddToRoleAsync(user, "client");
                return Created("", "Пользователь успешно создан.");
            }
            else
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
        }

        [HttpPost("Authentication")]
        public async Task<IActionResult> Authenticate(UserAuthorization model)
        {
            var user = await _userManager.FindByNameAsync(model.Login);
            var roleid = _context.UserRoles.First(x => x.UserId == user.Id).RoleId;
            var role = _context.Roles.First(x => x.Id == roleid).Name;
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID", user.Id.ToString()),
                        new Claim(ClaimTypes.Role, role)
                    }),

                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);


                return Ok(new { token });
            }
           
            else return BadRequest(new { message = "Username or password is incorrect." });
        }
    }
}