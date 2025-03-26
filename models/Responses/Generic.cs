namespace nehsanet_app.Models
{
    public class ApiResponseGeneric
    {
        public bool Success { get; set; }
        public object? Data { get; set; } = null;
    }
}