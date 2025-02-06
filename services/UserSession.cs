using nehsanet_app.db;
using nehsanet_app.Models;
using static nehsanet_app.Services.IUserSession;
using LogLevel = nehsanet_app.Models.LogLevel;

namespace nehsanet_app.Services
{
    public class UserSessionProvider : IUserSessionProvider
    {
        public string IP { get; set; } = "";
        public long SessionID { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserSessionProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetSession()
        {
            string ip = "";
            if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.Connection.RemoteIpAddress != null)
                ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            UserSession userSession = new()
            {
                IP = ip,
                SessionID = DateTime.UtcNow.Ticks
            };

            IP = userSession.IP;
            SessionID = userSession.SessionID;
        }
    }
}