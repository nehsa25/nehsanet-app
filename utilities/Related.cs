using System.Data.Common;
using MySqlConnector;
using nehsanet_app.Models;
using nehsanet_app.Services;

namespace nehsanet_app.utilities
{
    public class RelatedPagesUtility(MySqlDataSource database, ILoggingProvider logger)
    {
        private readonly ILoggingProvider _logger = logger;

        public async Task<List<RelatedPages>> GetRelatedPages(string pagename)
        {
            _logger.Log("Enter: GetRelatedPages/pagename [GET]");
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = @"SELECT stem, title FROM related_pages rp join pages p on rp.related_page_id = p.id WHERE rp.page_id = (select id from pages where stem = @Page)";
            //command.CommandText = @"SELECT stem, title FROM related_pages rp join pages p on rp.related_page_id = p.id WHERE rp.page_id = (select id from pages where stem = @Page) OR rp.related_page_id = (select id from pages where stem = @Page)";
            command.Parameters.AddWithValue("@Page", pagename);
            _logger.Log($"Interpolated command: {command.CommandText}");
            List<RelatedPages> result = await ReadRelatedPages(await command.ExecuteReaderAsync());
            _logger.Log("Enter: GetRelatedPages/pagename [GET]");
            return result;
        }

        private static async Task<List<RelatedPages>> ReadRelatedPages(DbDataReader reader)
        {
            var pages = new List<RelatedPages>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    string stem = reader.GetString(0);
                    string title = reader.GetString(1);
                    var post = new RelatedPages()
                    {
                        stem = stem,
                        title = title
                    };
                    pages.Add(post);
                }
            }
            return pages;
        }

        private static void BindParams(MySqlCommand cmd, RelatedPages relatedPages)
        {
            cmd.Parameters.AddWithValue("@page", relatedPages.stem);
        }
    }
}