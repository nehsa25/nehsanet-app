using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using nehsanet_app.utilities;
using nehsanet_app.Types;
using nehsanet_app.Models;
using nehsanet_app.Services;
using static nehsanet_app.utilities.ControllerUtility;
using nehsanet_app.db;
using Microsoft.EntityFrameworkCore;

namespace nehsanet_app.Controllers
{
    [ApiController]
    public class CommentsControler(ILoggingProvider logger, DataContext context) : ControllerBase
    {
        private readonly ILoggingProvider _logger = logger;
        private readonly DataContext _context = context;
        readonly List<NameAbout> names = [];

        [HttpGet]
        [Route("/v1/comment/{page}/{numberToReturn}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetComments(string pageName, int numberToReturn = 5)
        {
            ApiResponse response = new();
            response.Success = false;

            try
            {
                _logger.Log("Enter: GetComment/id [GET]");

                // we need to do this in EF
                // SELECT * FROM DBComments c
                // JOIN DBPages p on c.PageID = p.id
                // WHERE p.stem = pageName

                response.Data = await _context.DBComment.Include(c => c.Page)
                    .Where(c => c.Page.Stem == pageName)
                    .OrderByDescending(c => c.CommentID)
                    .Take(numberToReturn)
                    .ToListAsync();

                response.Success = true;
            }
            catch (Exception e)
            {
                _logger.Log(e, "GetComments");
            }

            _logger.Log($"Exit: GetComment/id. Response success? {response.Success}");
            return response;
        }

        [HttpPost]
        [Route("/v1/comment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> PostComment([FromBody] DBComment commentPost)
        {
            ApiResponse response = new();

            try
            {
                _logger.Log("Enter: PostComment [POST]");
                if (HttpContext.Connection.RemoteIpAddress != null) {
                    string ip = HttpContext.Connection.RemoteIpAddress.ToString();
                    _logger.Log($"Adding IP to commentPost: {ip}");
                    commentPost.IP = ip;
                }

                CommentsUtility commentsUtility = new(_logger, _context);
                response.Success = await commentsUtility.AddComment(commentPost);
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