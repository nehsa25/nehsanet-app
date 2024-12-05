using nehsanet_app.db;
using nehsanet_app.Models;
using LogLevel = nehsanet_app.Models.LogLevel;

namespace nehsanet_app.Services
{
    public class LoggingProvider : ILoggingProvider
    {
        private readonly DataContext _context;

        public LoggingProvider(DataContext context)
        {
            _context = context;
        }

        public void Log(string message, string user, string ip, int? level)
        {
            Log log = new()
            {
                Message = message,
                User = user,
                IP = ip,
                Date = DateTime.Now,
                Level = level ?? 1
            };
                                                    
            _context.Logs.Add(log);
            _context.SaveChanges();
        }
    }
}