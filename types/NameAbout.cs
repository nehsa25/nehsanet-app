namespace nehsanet_app.Types
{
    public class NameAbout(string Name, String About)
    {
        public string Name { get; set; } = Name;
        public string About { get; set; } = About;
    }

    public class NameType(string Name)
    {
        public string Name { get; set; } = Name;
    }

    public class ContactMeRequest(string Name)
    {
        public string Name { get; set; } = Name;
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public class ContactMeResponse()
    {
        public bool Success { get; set; } = true;
    }
}