namespace Nehsa.Controllers {
    public class NameAbout(string Name, String About)
    {
        public string Name { get; set; } = Name;
        public string About { get; set; } = About;
    }
}