namespace nehsanet_app.Services
{
    public interface ILoggingProvider
    {
        void Log(string message, int? level = 1);
        void Log(Exception ex, string message, int? level = 1);
    }
}