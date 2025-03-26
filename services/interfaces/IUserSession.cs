namespace nehsanet_app.Services
{
    public interface IUserSession
    {
        public interface IUserSessionProvider
        {
            public string IP { get; set; }
            public long SessionID { get; set; }
            public Task SetSession(System.Security.Claims.ClaimsPrincipal user);
        }
    }
}