using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Data;
using Dapper;

namespace nehsanet_app.Controllers;

public class MudRace
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Abilities { get; set; }
    public string? Directives { get; set; }
    public int? BaseExperienceAdjustment { get; set; }
    public bool? Playable { get; set; }
}

public class MudClass
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Abilities { get; set; }
    public string? Directives { get; set; }
    public int? BaseExperienceAdjustment { get; set; }
    public bool? Playable { get; set; }
}

[ApiController]
[Route("[controller]")]
public class MudController : ControllerBase
{
    private readonly string _dbPath;
    private readonly ILogger<MudController> _logger;

    public MudController(IConfiguration configuration, ILogger<MudController> logger)
    {
        _dbPath = configuration.GetValue<string>("SqliteDbPath") ?? throw new Exception("SqliteDbPath is not configured.");
        _logger = logger;

        if (!System.IO.File.Exists(_dbPath))
        {
            _logger.LogWarning($"Database file not found at {_dbPath}. Creating a new one.");
            System.IO.File.Create(_dbPath).Close();
        }
    }

    [HttpGet]
    [Route("/v1/mud/races")]
    public IActionResult GetRaces()
    {
        try
        {
            var sql = @"
                select 
                    id,
                    name,
                    description,
                    abilities,
                    directives,
                    base_experience_adjustment as BaseExperienceAdjustment,
                    playable
                from player_races;
            ";

            using IDbConnection connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            // Get all races
            IEnumerable<MudRace> mudRaces = connection.Query<MudRace>(sql);

            return Ok(mudRaces);
        }
        catch (SqliteException ex)
        {
            _logger.LogError($"SQLite Error: {ex.Message}");
            return BadRequest($"SQLite Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"General Error: {ex.Message}");
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    [HttpGet]
    [Route("/v1/mud/classes")]
    public IActionResult GetClasses()
    {
        try
        {
            var sql = @"
                select 
                    id,
                    name,
                    description,
                    abilities,
                    directives,
                    base_experience_adjustment as BaseExperienceAdjustment,
                    playable
                from player_classes;
            ";
            using IDbConnection connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            // Get all races
            IEnumerable<MudClass> mudClasses = connection.Query<MudClass>(sql);

            return Ok(mudClasses);
        }
        catch (SqliteException ex)
        {
            _logger.LogError($"SQLite Error: {ex.Message}"); // Log the error
            return BadRequest($"SQLite Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"General Error: {ex.Message}");
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}