using Api.Data;
using Api.DTOs;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    // https://localhost:7106/api/auth
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;

        public AuthController(AppDbContext context, IConfiguration config, IEmailService emailService)
        {
            _context = context;
            _config = config;
            _emailService = emailService;
        }

        [HttpPost("register")]
        // https://localhost:7106/api/auth/register
        public async Task<IActionResult> Register(RegisterDto request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest(new { message = "Email is already registered." });
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var newUser = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = passwordHash,
                Rol = "User", 
                VerificationToken = Guid.NewGuid().ToString("N")
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            string emailHtml = $@"
                <div style='font-family: Arial, sans-serif; padding: 20px; text-align: center;'>
                    <h2 style='color: #4CAF50;'>Welcome to our Store! 🎉</h2>
                    <p>Hello, we are very happy to have you join us.</p>
                    <p>You are just one step away from starting to shop. Please confirm your account by clicking the button below:</p>
                    <br>
                    <a href='http://localhost:4200/verify-email?token={newUser.VerificationToken}' style='background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; font-weight: bold;'>
                        Confirm my account
                    </a>
                    <br><br>
                    <p style='color: #777; font-size: 12px;'>If you didn't create this account, you can safely ignore this email.</p>
                </div>
            ";

            try
            {
                await _emailService.EnviarCorreoAsync(request.Email, "Welcome! Please confirm your account", emailHtml);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }

            return Ok(new { message = "User registered successfully." });
        }


        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.VerificationToken == request.Token);
            if (user == null)
            {
                return BadRequest(new { message = "Invalid token or account already verified." });
            }
            user.IsVerified = true;
            user.VerificationToken = null;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Account successfully verified! You can now sign in." });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return BadRequest(new { message = "Invalid credentials." });
            }

            if (!user.IsVerified)
            {
                return Unauthorized(new { message = "You must verify your email before signing in. Please check your inbox." });
            }

            var key = Encoding.ASCII.GetBytes(_config.GetSection("Jwt:Key").Value!);
            var claims = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Rol ?? "User")
            });

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new { token = tokenHandler.WriteToken(token) });
        }

    }
}