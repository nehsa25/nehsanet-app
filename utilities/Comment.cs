using System.Data.Common;
using MySqlConnector;
using nehsanet_app.Models;

namespace nehsanet_app.utilities
{
    public class CommentsUtility(MySqlDataSource database, ILogger? _logger = null)
    {
        public async Task<DBComment?> GetSingleCommentById(int commentid)
        {
            _logger?.LogInformation("Enter: GetComment/id [GET]");
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = @"SELECT `commentid`, `username`, `comment` FROM `comments` WHERE `commentid` = @commentid";
            command.Parameters.AddWithValue("@commentid", commentid);
            dynamic result = await GetComments(await command.ExecuteReaderAsync());
            _logger?.LogInformation("Enter: GetComment/id [GET]");
            return result.FirstOrDefault();
        }

        public async Task<IReadOnlyList<DBComment>> GetLastXCommentsByPage(string page, int numberToReturn)
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = $"SELECT `commentid`, `username`, `comment` FROM `comments` WHERE page = '{page}' ORDER BY `commentid` DESC LIMIT {numberToReturn};";
            return await GetComments(await command.ExecuteReaderAsync());
        }

        public async Task DeleteAllComments()
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM `comments`";
            await command.ExecuteNonQueryAsync();
        }

        public async Task AddComment(DBComment CommentPost)
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO `comments` (`username`, `comment`, `ip_address`, `date`, `page`) VALUES (@username, @comment, @ip_address, @date, @page);";
            BindParams(command, CommentPost);
            await command.ExecuteNonQueryAsync();
        }

        // public async Task UpdateComment(CommentPost CommentPost)
        // {
        //     using var connection = await database.OpenConnectionAsync();
        //     using var command = connection.CreateCommand();
        //     command.CommandText = @"UPDATE `comments` SET `username` = @username, `comment` = @comment WHERE `commentid` = @commentid;";
        //     BindParams(command, CommentPost);
        //     await command.ExecuteNonQueryAsync();
        // }

        // public async Task DeleteComment(CommentPost CommentPost)
        // {
        //     using var connection = await database.OpenConnectionAsync();
        //     using var command = connection.CreateCommand();
        //     command.CommandText = @"DELETE FROM `comments` WHERE `commentid` = @commentid;";
        //     await command.ExecuteNonQueryAsync();
        // }

        private static async Task<IReadOnlyList<DBComment>> GetComments(DbDataReader reader)
        {
            var posts = new List<DBComment>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    int commentid = reader.GetInt32(0);
                    string username = reader.GetString(1);
                    string comment = reader.GetString(2);
                    var post = new DBComment(username, comment, commentid: commentid, page: "");
                    posts.Add(post);
                }
            }
            return posts;
        }

        private static void BindParams(MySqlCommand cmd, DBComment CommentPost)
        {
            cmd.Parameters.AddWithValue("@username", CommentPost.Username);
            cmd.Parameters.AddWithValue("@comment", CommentPost.Comment);
            cmd.Parameters.AddWithValue("@page", CommentPost.Page);
            cmd.Parameters.AddWithValue("@date", CommentPost.Date);
            cmd.Parameters.AddWithValue("@ip_address", CommentPost.IP);
        }
    }
}