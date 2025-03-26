using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nehsanet_app.db;
using nehsanet_app.Models;
using nehsanet_app.Types;

namespace nehsanet_app.Controllers
{
    [ApiController]
    public class NamesController(
        DataContext context, ILogger<NamesController> logger,
        IHttpContextAccessor httpContextAccessor
        ) : ControllerBase
    {
        private readonly ILogger _logger = logger;
        private readonly DataContext _context = context;
        readonly List<NameAbout> names = [];

        [HttpGet] 
        [Route("/v1/names")]
        public async Task<ActionResult<ApiResponseGeneric>> GetNames()
        {
            ApiResponseGeneric response = new();
            response.Success = false;

            try
            {
                response.Data = await _context.DBName.Select(_ => _.Name).ToListAsync();
                response.Success = true;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e, "GetNames");
            }

            _logger.LogInformation("Exit: GetNames. Response success: {s}", response.Success);
            return response;
        }

        [HttpGet]
        [Route("/v1/name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponseGeneric>> GetName()
        {
            ApiResponseGeneric response = new();
            response.Success = false;

            try
            {
                _logger.LogInformation("Enter: names() [GET]");
                response.Data = await _context.DBName.Select(_ => _.Name).FirstOrDefaultAsync() ?? "";
                response.Success = true;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e, "GetName");
            }

            _logger.LogInformation("Exit: GetName. Response success: {s}", response.Success);
            return response;
        }

        [HttpGet]
        [Route("/v1/name/{numToReturn}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponseName>> GetNames(int numToReturn)
        {
            ApiResponseName response = new()
            {
                Success = false,
                Names = []
            };

            try
            {
                _logger.LogInformation("Enter: names() [GET]");
                List<NameAbout> names = await _context.DBName
                    .Select(name => new NameAbout(name.Name, name.Description))
                    .ToListAsync();

                // Take two random names from the list
                int founditems = 0;
                List<NameAbout> items = [];
                while (founditems < numToReturn)
                {
                    dynamic item = names[Random.Shared.Next(names.Count)];
                    if (!items.Contains(item))
                    {
                        items.Add(item);
                        founditems++;
                    }
                }
                response.Names = items;
                response.Success = true;
                response.IP = httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "";
            }
            catch (Exception e)
            {
                _logger.LogInformation(e, "GetNames");
            }

            _logger.LogInformation("Exit: GetNames. Response success: {r}", response.Success);
            return response;
        }
    }
}