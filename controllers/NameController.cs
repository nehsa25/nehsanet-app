using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nehsanet_app.db;
using nehsanet_app.Types;

namespace nehsanet_app.Controllers
{
    [ApiController]
    public class NamesController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly DataContext _context;
        readonly List<NameAbout> names = [];

        public NamesController(DataContext context, ILogger<NamesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet] // to access this: /Names
        [Route("/v1/names")]
        public async Task<ActionResult<IEnumerable<string>>> GetNames()
        {
            return await _context.DBName.Select(_ => _.Name).ToListAsync();
        }

        [HttpGet]
        [Route("/v1/name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<string>>  GetName()
        {
            _logger.LogInformation("Enter: names() [GET]");
            string jsonresults = await _context.DBName.Select(_ => _.Name).FirstOrDefaultAsync() ?? "";
            _logger.LogInformation($"Exit: names(): results: ${jsonresults}");
            return jsonresults;
        }

        [HttpGet]
        [Route("/v1/name/{numToReturn}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<NameAbout>>> GetNames(int numToReturn)
        {
            _logger.LogInformation("Enter: names() [GET]");
            dynamic jsonresults = await _context.DBName.Select(_ => new NameAbout(_.Name, _.Description)).Take(numToReturn).ToListAsync();            
            _logger.LogInformation($"Exit: names(): results: ${JsonSerializer.Serialize(jsonresults)}");
            return jsonresults;
        }
    }
}