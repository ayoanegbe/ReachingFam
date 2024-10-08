using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ReachingFam.Core.Enums;
using ReachingFam.Core.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ReachingFam.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public AuthResponse Login([FromBody] AuthRequest request)
        {
            AuthResponse response = new();
            if (request.ApiUserId == Guid.NewGuid() && request.Password == "password") // Simple authentication logic
            {
                response.Status = ResponseStatus.Success;
                return response;
            }

            return response;
        }        
              
    }
}
