using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTOs.AuthDTO;
using server.Helpers;
using server.Models;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly JwtHelper _jwtHelper;

        public AuthController(AppDbContext dbContext, JwtHelper jwtHelper)
        {
           _dbContext = dbContext;
            _jwtHelper = jwtHelper;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDTO loginDTO) {
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.UserName == loginDTO.UserName);

            if (user == null) {
                return BadRequest("User does not exist");
            }

            
            if (!VerifyPassword(loginDTO.Password,user.HashedPassword)) {
                return BadRequest("Username or Password is Invalid");
            }

            List<string> roles = ["Admin", "User", "Manager"];
            string token = _jwtHelper.GenerateToken(user, roles);

            return Ok(token);
        }


        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDTO userDTO)
        {
            //unique email
            if (await _dbContext.Users.AnyAsync(u => u.UserEmail == userDTO.Email))
            {
                return BadRequest("User Already Exists");
            }
            

            var user = new User
            {
                UserEmail = userDTO.Email,
                UserName = userDTO.UserName,
                HashedPassword = HashPassword(userDTO.Password),
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return Ok(user);
        }

        [Authorize]
        [HttpGet("test")]
        public string Test()
        {
            return "It is working";
        }
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string text,string hash)
        {
            return BCrypt.Net.BCrypt.Verify(text, hash);
        }
    }
}
