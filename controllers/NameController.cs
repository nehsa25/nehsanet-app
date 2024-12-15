using System.Runtime.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nehsanet_app.db;
using nehsanet_app.Services;
using nehsanet_app.Types;
using static nehsanet_app.utilities.ControllerUtility;

namespace nehsanet_app.Controllers
{
    [ApiController]
    public class NamesController(DataContext context, ILoggingProvider logger) : ControllerBase
    {
        private readonly ILoggingProvider _logger = logger;
        private readonly DataContext _context = context;
        readonly List<NameAbout> names = [];

        [HttpGet] // to access this: /Names
        [Route("/v1/names")]
        public async Task<ActionResult<ApiResponse>> GetNames()
        {
            ApiResponse response = new();
            response.Success = false;

            try
            {
                response.Data = await _context.DBName.Select(_ => _.Name).ToListAsync();
                response.Success = true;
            }
            catch (Exception e)
            {
                _logger.Log(e, "GetNames");
            }

            _logger.Log($"Exit: GetNames. Response success? {response.Success}");
            return response;
        }

        [HttpGet]
        [Route("/v1/name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetName()
        {
            ApiResponse response = new();
            response.Success = false;

            try
            {
                _logger.Log("Enter: names() [GET]");
                response.Data = await _context.DBName.Select(_ => _.Name).FirstOrDefaultAsync() ?? "";
                response.Success = true;
            }
            catch (Exception e)
            {
                _logger.Log(e, "GetName");
            }

            _logger.Log($"Exit: GetName. Response success? {response.Success}");
            return response;
        }

        [HttpGet]
        [Route("/v1/name/{numToReturn}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetNames(int numToReturn)
        {
            ApiResponse response = new();
            response.Success = false;

            try
            {
                await Task.Run(() =>
                {
                    _logger.Log("Enter: names() [GET]");
                    List<NameAbout> names = (from name in _context.DBName
                                             select new NameAbout(name.Name, name.Description)).ToList();

                    // Take two random names from the list
                    int founditems = 0;
                    List<dynamic> items = [];
                    while (founditems < numToReturn)
                    {
                        dynamic item = names[Random.Shared.Next(names.Count)];
                        if (!items.Contains(item))
                        {
                            items.Add(item);
                            founditems++;
                        }
                    }
                    response.Data = JsonSerializer.Serialize(items);
                    response.Success = true;
                });
            }
            catch (Exception e)
            {
                _logger.Log(e, "GetNames");
            }

            _logger.Log($"Exit: GetNames. Response success? {response.Success}");
            return response;
        }
    }
}