namespace nehsanet_app.Models
{
    public class CommentPost(string username, string comment, string? page, int? commentid = null, string? ip = null)
    {
        public int? commentid { get; set; } = commentid;
        public string username { get; set; } = username;
        public string comment { get; set; } = comment;
        public string page { get; set; } = page ?? "";
        public string ip { get; set; } = ip ?? "";
        public DateTime date { get; set; } = DateTime.Now;
    }
}