using EmployeeTimeTracking_API.DTOs;
using EmployeeTimeTracking_API.IService;
using EmployeeTimeTracking_API.Model;
using EmployeeTimeTracking_API.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTimeTracking_API.Service
{
    public class AuthService : IAuthService
    {
        public readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<RegisterUserResponseDto>> RegisterUserAsync(RegisterUserRequestDto request)
        {
            var response = new ServiceResponse<RegisterUserResponseDto>();

            // 1. Check if user already exists
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                response.Success = false;
                response.Message = "User with this email already exists.";
                return response;
            }

            // 2. Hash the password
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // 3. Create the new User model
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = passwordHash,
                DesignationId = request.DesignationId,

                // Default Role = Employee (Assuming RoleId: 1 = Admin, 2 = Employee)
                RoleId = 2
            };

            // 4. Save to database
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Now that the user is saved (and has a UserId), we can generate a token!

            // We need to fetch the Role for the token claims (since we just set RoleId=2)
            // Ideally, load the role name, or just hardcode "Employee" for the claim since we know it's 2.
            user.Role = await _context.Roles.FindAsync(2);

            string token = GenerateToken(user);

            // 5. Prepare Response with Token
            response.Data = new RegisterUserResponseDto
            {
                UserId = user.UserId,
                Email = user.Email,
                FullName = $"{user.FirstName} {user.LastName}",
                Token = token // <--- Set the token here!
            };
            response.Message = "User registered and logged in successfully.";

            return response;

        }

        public async Task<ServiceResponse<LoginResponseDto>> LoginUserAsync(LoginRequestDto request)
        {
            var response = new ServiceResponse<LoginResponseDto>();

            // 1. Find user by email
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                response.Success = false;
                response.Message = "Invalid  email or password.";
                return response;
            }

            // 2. Verify the password
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                response.Success = false;
                response.Message = "Invalid email or password.";
                return response;
            }

            // 3. Generate JWT Token
            string token = GenerateToken(user);

            string fName = user.FirstName;
            string lName = user.LastName;

            // You can use them for anything
            string fullName = $"{fName} {lName}";

            // 4. Send Response
            response.Data = new LoginResponseDto
            {

                Token = token,
                Email = user.Email,
                Role = user.Role.Name,
                FullName = fullName
            };
            response.Message = "Login successful.";

            return response;
        }

        // --- Priv ate Helper Method to Create the Token ---
        private string GenerateToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),

                // Required for backend role-based authorization
                new Claim(ClaimTypes.Role, user.Role.Name),

                // Optional but useful for frontend checks
                new Claim("role", user.Role.Name),

                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

