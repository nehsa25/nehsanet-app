using System.Data.Common;
using MySqlConnector;
using nehsanet_app.Models;

public class CommentsRepository(MySqlDataSource database, ILogger? _logger = null)
{
    public async Task<CommentPost?> GetSingleCommentById(int commentid)
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

    public async Task<IReadOnlyList<CommentPost>> GetLastXCommentsByPage(string page, int numberToReturn)
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

    public async Task AddComment(CommentPost CommentPost)
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

    private static async Task<IReadOnlyList<CommentPost>> GetComments(DbDataReader reader)
    {
        var posts = new List<CommentPost>();
        using (reader)
        {
            while (await reader.ReadAsync())
            {
                int commentid = reader.GetInt32(0);
                string username = reader.GetString(1);
                string comment = reader.GetString(2);
                var post = new CommentPost(username, comment, commentid: commentid, page: "");
                posts.Add(post);
            }
        }
        return posts;
    }

    private static void BindParams(MySqlCommand cmd, CommentPost CommentPost)
    {
        cmd.Parameters.AddWithValue("@username", CommentPost.username);
        cmd.Parameters.AddWithValue("@comment", CommentPost.comment);
        cmd.Parameters.AddWithValue("@page", CommentPost.page);
        cmd.Parameters.AddWithValue("@date", CommentPost.date);
        cmd.Parameters.AddWithValue("@ip_address", CommentPost.ip);
    }
}