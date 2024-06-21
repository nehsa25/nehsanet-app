namespace nehsanet_app.Models
{
    public class CommentPost
    {
        public int commentid { get; set; }
        public string username { get; set; }
        public string comment { get; set; }
        public DateTime date { get; set; }

        public CommentPost(int commentid, string username, string comment)
        {
            this.commentid = commentid;
            this.username = username;
            this.comment = comment;
        }

    }
}