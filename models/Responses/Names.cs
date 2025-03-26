using nehsanet_app.Types;

namespace nehsanet_app.Models
{
    public class ApiResponseName
    {
        public bool Success { get; set; }

        required
        public List<NameAbout> Names { get; set; }

        public string? IP { get; set; }
    }

}