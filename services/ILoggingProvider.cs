namespace nehsanet_app.Services
{
    public interface ILoggingProvider
    {
        void Log(string message, string user="", string ip="", int? level = 1);
    }
}