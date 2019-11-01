using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Database.Models
{
    public class CurrentUserInfo : ClaimsIdentity
    {
        public string Id { get; set; }
        public string Role { get; set; }
        public CurrentUserInfo(IHttpContextAccessor accessor)
        {
            var identity = accessor.HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = identity.Claims;
            Id = claims.First(x => x.Type == "UserID").Value;
            Role = claims.First(x => x.Type == ClaimTypes.Role).Value;
        }
    }
}
