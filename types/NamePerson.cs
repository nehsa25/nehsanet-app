namespace Nehsa.Controllers {
    public class NamePerson(string name, String about)
    {
        public string Name { get; set; } = name;
        public string About { get; set; } = about;
    }
}