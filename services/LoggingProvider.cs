using nehsanet_app.db;
using nehsanet_app.Models;
using static nehsanet_app.Services.IUserSession;
using LogLevel = nehsanet_app.Models.LogLevel;

namespace nehsanet_app.Services
{
    public class LoggingProvider : ILoggingProvider
    {
        private readonly DataContext _context;
        private readonly IUserSessionProvider _userSession;

        public LoggingProvider(DataContext context, IUserSessionProvider userSession)
        {
            _context = context;
            _userSession = userSession;
        }

        public void Log(string message, int? level)
        {
            if (_userSession.IP == null || _userSession.SessionID == 0)
                _userSession.SetSession();

            Log log = new()
            {
                Message = message,
                IP = _userSession.IP ?? "",
                SessionID = _userSession.SessionID,
                Date = DateTime.Now,
                Log_LogLevelID = level ?? 1
            };
            Console.WriteLine(message);                                                    
            _context.Logs.Add(log);
            _context.SaveChanges();
        }

        public void Log(Exception ex, string message, int? level)
        {
            Log($"ERROR: {message} - {ex.Message}", level);
        }
    }
}