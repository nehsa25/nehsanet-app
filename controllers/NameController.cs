using System.Runtime.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nehsanet_app.db;
using nehsanet_app.Services;
using nehsanet_app.Types;

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
        public async Task<ActionResult<IEnumerable<string>>> GetNames()
        {
            return await _context.DBName.Select(_ => _.Name).ToListAsync();
        }

        [HttpGet]
        [Route("/v1/name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> GetName()
        {
            _logger.Log("Enter: names() [GET]");
            string jsonresults = await _context.DBName.Select(_ => _.Name).FirstOrDefaultAsync() ?? "";
            _logger.Log($"Exit: names(): results: {jsonresults}");
            return jsonresults;
        }

        [HttpGet]
        [Route("/v1/name/{numToReturn}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public List<dynamic> GetNames(int numToReturn)
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
            dynamic jsonresults = JsonSerializer.Serialize(items);

            _logger.Log($"Exit: names(): results: ${JsonSerializer.Serialize(items)}");
            return items;
        }
    }
}