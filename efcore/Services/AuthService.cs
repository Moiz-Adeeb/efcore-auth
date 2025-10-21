using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace efcore.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration configuration;
        private readonly DataContext context;

        public AuthService(IConfiguration configuration, DataContext context)
        {
            this.configuration = configuration;
            this.context = context;
        }

        public async Task<UserOutputDto?> RegisterUserAsync(UserInputDto request)
        {
            if (await context.Users.AnyAsync(u => u.Username == request.Username))
            {
                return null;
            }
            var user = new User();
            user.Username = request.Username;
            user.PasswordHash = new PasswordHasher<User>()
                .HashPassword(user, request.Password);
            user.Roles = "User";
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return user.ToDto();
        }

        public async Task<UserOutputDto?> RegisterManagerAsync(UserInputDto request)
        {
            if (await context.Users.AnyAsync(u => u.Username == request.Username))
            {
                return null;
            }
            var user = new User();
            user.Username = request.Username;
            user.PasswordHash = new PasswordHasher<User>()
                .HashPassword(user, request.Password);
            user.Roles = "Manager";
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return user.ToDto();
        }

        public async Task<string?> LoginAsync(UserInputDto request)
        {
            User? user = await context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user is null)
            {
                return null;
            }
            if (new PasswordHasher<User>()
                .VerifyHashedPassword(user, user.PasswordHash, request.Password)
               == PasswordVerificationResult.Failed)
            {
                return null;
            }
            string token = CreateToken(user);
            return token;
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Roles)
            };
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
