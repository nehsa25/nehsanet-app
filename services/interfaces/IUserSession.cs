using nehsanet_app.Models;

namespace nehsanet_app.Services
{
    public interface IUserSession
    {
        public interface IUserSessionProvider
        {
            public string IP { get; set; }
            public long SessionID { get; set; }
            public void SetSession();
        }
    }
}