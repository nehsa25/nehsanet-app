using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using nehsanet_app.utilities;
using nehsanet_app.Types;
using nehsanet_app.Models;
using nehsanet_app.Services;
using static nehsanet_app.utilities.ControllerUtility;

namespace nehsanet_app.Controllers
{
    [ApiController]
    public class CommentsControler(ILoggingProvider logger) : ControllerBase
    {
        private readonly ILoggingProvider _logger = logger;

        readonly List<NameAbout> names = [];

        [HttpGet]
        [Route("/v1/comment/{page}/{numberToReturn}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetComment([FromServices] MySqlDataSource db, string page, int numberToReturn = 5)
        {
            ApiResponse response = new();

            try
            {
                _logger.Log("Enter: GetComment/id [GET]");
                var connection = new CommentsUtility(db, _logger);
                List<DBComment> db_result = (List<DBComment>)await connection.GetLastXCommentsByPage(page, numberToReturn);
                response.Success = true;
                response.Data = JsonSerializer.Serialize(db_result);
                _logger.Log($"Exit: GetComment/id: results: ${JsonSerializer.Serialize(response)}");
            }
            catch (Exception e)
            {
                _logger.Log($"Error: GetComment: {e.Message}");
                return new ApiResponse { Success = false };
            }

            _logger.Log($"Exit: GetComment/id. Response success? {response.Success}");
            return response;
        }

        [HttpPost]
        [Route("/v1/comment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> PostComment([FromServices] MySqlDataSource db, [FromBody] DBComment commentPost)
        {
            ApiResponse response = new();

            try
            {
                _logger.Log("Enter: PostComment [POST]");
                var connection = new CommentsUtility(db, _logger);
                if (HttpContext.Connection.RemoteIpAddress != null)
                    commentPost.IP = HttpContext.Connection.RemoteIpAddress.ToString();
                await connection.AddComment(commentPost);
                response.Success = true;
            }
            catch (Exception e)
            {
                _logger.Log($"Error: PostComment: {e.Message}");
                response.Success = false;
            }

            _logger.Log($"Exit: PostComment. Response: {JsonSerializer.Serialize(response)}");
            return response;
        }
    }
}