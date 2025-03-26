using System.Security.Claims;
using nehsanet_app.Models;
using static nehsanet_app.Services.IUserSession;
using nehsanet_app.db;
using Microsoft.EntityFrameworkCore;

namespace nehsanet_app.Services
{
    public class UserSessionProvider : IUserSessionProvider
    {
        public string IP { get; set; } = "";
        public long SessionID { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dbContext;

        public UserSessionProvider(IHttpContextAccessor httpContextAccessor, DataContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        public async Task SetSession(ClaimsPrincipal user)
        {
            string ip = "";
            if (_httpContextAccessor.HttpContext != null &&
                _httpContextAccessor.HttpContext.Connection.RemoteIpAddress != null)
                ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            UserSession userSession = new()
            {
                IP = ip,
                SessionID = DateTime.UtcNow.Ticks
            };

            IP = userSession.IP;
            SessionID = userSession.SessionID;

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim != null)
            {
                var email = userIdClaim.Value;
                var appUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

                if (appUser != null)
                {
                    var claimsIdentity = (ClaimsIdentity)user.Identity!;
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, appUser.Role?.RoleLevel ?? ""));
                }
                else
                {
                    // User not found in the database, assign "New" role
                    var claimsIdentity = (ClaimsIdentity)user.Identity!;
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "New"));
                }
            }
            else
            {
                // No email claim, assign "New" role
                var claimsIdentity = (ClaimsIdentity)user.Identity!;
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "New"));
            }
        }
    }
}