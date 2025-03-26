using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using nehsanet_app.utilities;
using nehsanet_app.Types;
using nehsanet_app.Models;
using nehsanet_app.db;
using Microsoft.EntityFrameworkCore;

namespace nehsanet_app.Controllers
{
    [ApiController]
    public class CommentsControler(ILogger<CommentsControler> logger, DataContext context) : ControllerBase
    {
        private readonly ILogger _logger = logger;
        private readonly DataContext _context = context;
        readonly List<NameAbout> names = [];

        [HttpGet]
        [Route("/v1/comment/{numberToReturn}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponseGeneric>> GetComments([FromQuery] string stem, int numberToReturn = 5)
        {
            ApiResponseGeneric response = new();
            response.Success = false;

            try
            {
                _logger.LogInformation("Enter: GetComment/id [GET]");

                if (string.IsNullOrWhiteSpace(stem))
                    throw new Exception("Stem is required for GetComments");

                if (!stem.StartsWith("/"))
                    stem = "/" + stem;

                response.Data = await _context.DBComment.Include(c => c.PageNavigation)
                    .Where(c => c.PageNavigation.Stem == stem)
                    .OrderByDescending(c => c.CommentID)
                    .Take(numberToReturn)
                    .ToListAsync();

                response.Success = true;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e, "GetComments");
            }

            _logger.LogInformation("Exit: GetComment/id. Response success: {success}", response.Success);
            return response;
        }

        [HttpPost]
        [Route("/v1/comment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponseGeneric>> PostComment([FromBody] DBComment commentPost)
        {
            ApiResponseGeneric response = new();

            try
            {
                _logger.LogInformation("Enter: PostComment [POST]");
                if (HttpContext.Connection.RemoteIpAddress != null)
                {
                    string ip = HttpContext.Connection.RemoteIpAddress.ToString();
                    _logger.LogInformation("Adding IP to commentPost: {ip}", ip);
                    commentPost.IP = ip;
                }

                // add page id
                if (commentPost.PageID == 0)
                {
                    _logger.LogInformation("Finding page ID for commentPost");
                    commentPost.PageID = await _context.DBPage
                        .Where(p => p.Stem == commentPost.Stem)
                        .Select(p => p.Id)
                        .FirstOrDefaultAsync();
                }

                CommentsUtility commentsUtility = new(_logger, _context);
                _context.DBComment.Add(commentPost);
                int recordsAffected = await _context.SaveChangesAsync();
                response.Success = recordsAffected > 0;
            }
            catch (Exception e)
            {
                _logger.LogInformation("Error: PostComment: {message}", e.Message);
                response.Success = false;
            }

            _logger.LogInformation("Exit: PostComment. Response: {response}", JsonSerializer.Serialize(response));
            return response;
        }
    }
}