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
    public int? Strength { get; set; }
    public int? Intelligence { get; set; }
    public int? Wisdom { get; set; }
    public int? Charisma { get; set; }
    public int? Constitution { get; set; }
    public int? Dexterity { get; set; }
    public int? Luck { get; set; }
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
    public int? Strength { get; set; }
    public int? Intelligence { get; set; }
    public int? Wisdom { get; set; }
    public int? Charisma { get; set; }
    public int? Constitution { get; set; }
    public int? Dexterity { get; set; }
    public int? Luck { get; set; }
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
                    pr.id,
                    pr.name,
                    pr.description,
                    pr.abilities,
                    pr.directives,
                    pr.base_experience_adjustment as BaseExperienceAdjustment,
                    pr.playable,
                    a.strength,
                    a.intelligence,
                    a.wisdom,
                    a.charisma,
                    a.constitution,
                    a.dexterity,
                    a.luck
                from player_races pr
                LEFT JOIN attributes a ON pr.attributes_id = a.id;
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
                SELECT 
                    mc.id,
                    mc.name,
                    mc.description,
                    mc.abilities,
                    mc.directives,
                    mc.base_experience_adjustment as BaseExperienceAdjustment,
                    mc.playable,
                    a.strength,
                    a.intelligence,
                    a.wisdom,
                    a.charisma,
                    a.constitution,
                    a.dexterity,
                    a.luck
                FROM player_classes mc
                LEFT JOIN attributes a ON mc.attributes_id = a.id;
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