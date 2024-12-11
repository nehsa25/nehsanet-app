using System.Data.Common;
using MySqlConnector;
using nehsanet_app.Models;
using nehsanet_app.Services;

namespace nehsanet_app.utilities
{
    public class ControllerUtility(ILoggingProvider logger)
    {
        private readonly ILoggingProvider _logger = logger;

        public class ApiResponse
        {
            public bool Success { get; set; }
            public object? Data { get; set; } = null;
        }
    }
}