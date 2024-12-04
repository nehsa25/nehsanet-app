
using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using nehsanet_app.models;
using nehsanet_app.utilities;
using nehsanet_app.Types;

namespace nehsanet_app.Controllers
{
    [ApiController]
    public class CommentsControler : ControllerBase
    {
        private readonly ILogger _logger;

        readonly List<NameAbout> names = [];

        public CommentsControler(ILogger<CommentsControler> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("/v1/comment/{page}/{numberToReturn}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<string> GetComment([FromServices] MySqlDataSource db, string page, int numberToReturn = 5)
        {
            _logger?.LogInformation("Enter: GetComment/id [GET]");
            var connection = new CommentsUtility(db, _logger);
            List<DBComment> db_result = (List<DBComment>)await connection.GetLastXCommentsByPage(page, numberToReturn);
            dynamic results = JsonSerializer.Serialize(db_result);
            _logger?.LogInformation($"Exit: GetComment/id: results: ${results}");
            return results;
        }

        [HttpPost]
        [Route("/v1/comment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<string> PostComment([FromServices] MySqlDataSource db, [FromBody] DBComment commentPost)
        {
            _logger?.LogInformation("Enter: PostComment [POST]");
            var connection = new CommentsUtility(db, _logger);
            if (HttpContext.Connection.RemoteIpAddress != null)
                commentPost.IP = HttpContext.Connection.RemoteIpAddress.ToString();
            await connection.AddComment(commentPost);
            dynamic results = JsonSerializer.Serialize<string>("OK");
            _logger?.LogInformation($"Exit: PostComment: results: ${results}");
            return results;
        }
    }
}