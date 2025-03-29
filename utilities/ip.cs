using System.Net;

namespace nehsanet_app.Utilities
{
    public static class IpHelper
    {
        public static string GetClientIpAddress(HttpContext? context)
        {
            string? ipAddress = context?.Request?.Headers["X-Forwarded-For"].FirstOrDefault();

            if (string.IsNullOrEmpty(ipAddress))
                ipAddress = context?.Request?.Headers["X-Real-IP"].FirstOrDefault();

            if (string.IsNullOrEmpty(ipAddress))
                ipAddress = context?.Connection.RemoteIpAddress?.ToString();

            if (!string.IsNullOrEmpty(ipAddress) && ipAddress.Contains(','))
                ipAddress = ipAddress.Split(',')[0].Trim();

            if (IPAddress.TryParse(ipAddress, out _))
                return ipAddress;

            return "";
        }
    }
}