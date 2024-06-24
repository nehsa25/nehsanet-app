namespace nehsanet_app.Models
{
    public class CommentPost
    {
        public int? CommentId { get; set; }
        public string Username { get; set; }
        public string Comment { get; set; }
        public string Page { get; set; }
         public string Ip { get; set; }
        public DateTime CreationDate { get; set; }

        public CommentPost(string username, string comment, string page, int? commentid=null)
        {
            this.Username = username;
            this.Comment = comment;
            this.Page = page;
            this.Ip = "";
            this.CreationDate = DateTime.Now;

        }

    }
}