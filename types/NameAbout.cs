namespace nehsanet_app.Types {
    public class NameAbout(string Name, String About)
    {
        public string Name { get; set; } = Name;
        public string About { get; set; } = About;
    }

        public class NameType(string Name)
    {
        public string Name { get; set; } = Name;
    }
}