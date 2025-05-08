using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Extensions.Logging; // Add this for logging

[ApiController]
[Route("[controller]")]
public class SqliteSchemaController : ControllerBase
{
    private readonly string _dbPath;
    private readonly ILogger<SqliteSchemaController> _logger;

    public SqliteSchemaController(IConfiguration configuration, ILogger<SqliteSchemaController> logger)
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
    [Route("/v1/sqlite/gettableschemas")]
    public IActionResult GetTableSchemas()
    {
        try
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            // Get all table names
            List<string> tableNames = GetTableNames(connection);

            // Get schema for each table
            List<TableSchema> tableSchemas = tableNames.Select(tableName => GetTableSchema(connection, tableName)).ToList();

            return Ok(tableSchemas);
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

    private List<string> GetTableNames(SqliteConnection connection)
    {
        List<string> tableNames = new List<string>();
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%';"; // Exclude sqlite system tables.
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    tableNames.Add(reader.GetString("name"));
                }
            }
        }
        return tableNames;
    }

    private TableSchema GetTableSchema(SqliteConnection connection, string tableName)
    {
        TableSchema tableSchema = new TableSchema { Name = tableName };
        tableSchema.Schema = new List<ColumnSchema>();

        using (var command = connection.CreateCommand())
        {
            command.CommandText = $"PRAGMA table_info({tableName})"; // Use PRAGMA table_info
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    // Log the entire row for debugging
                    string rowData = "";
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        rowData += $"{reader.GetName(i)}: {reader.GetValue(i)}, ";
                    }
                    _logger.LogInformation($"Table: {tableName}, Row: {rowData}");

                    tableSchema.Schema.Add(new ColumnSchema
                    {
                        ColumnName = reader.GetString("name"),
                        ColumnType = reader.GetString("type"),
                        NotNull = reader.GetBoolean("notnull"),
                        DefaultValue = reader.IsDBNull(reader.GetOrdinal("dflt_value")) ? null : reader.GetValue(reader.GetOrdinal("dflt_value")),
                        PrimaryKey = reader.GetInt32("pk"),
                    });
                }
            }
        }

        // Get foreign key info
        tableSchema.ForeignKeys = GetForeignKeys(connection, tableName);

        // Get index info
        tableSchema.Indices = GetIndices(connection, tableName);

        return tableSchema;
    }

    private List<ForeignKeySchema> GetForeignKeys(SqliteConnection connection, string tableName)
    {
        List<ForeignKeySchema> foreignKeys = new List<ForeignKeySchema>();
        using (var command = connection.CreateCommand())
        {
            command.CommandText = $"PRAGMA foreign_key_list({tableName})";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    foreignKeys.Add(new ForeignKeySchema
                    {
                        FromColumn = reader.GetString("from"),
                        ToTable = reader.GetString("table"),
                        ToColumn = reader.GetString("to"),
                        OnUpdate = reader.GetString("on_update"),
                        OnDelete = reader.GetString("on_delete"),
                        Match = reader.GetString("match")
                    });
                }
            }
        }
        return foreignKeys;
    }

    private List<IndexSchema> GetIndices(SqliteConnection connection, string tableName)
    {
        List<IndexSchema> indices = new List<IndexSchema>();
        using (var command = connection.CreateCommand())
        {
            command.CommandText = $"PRAGMA index_list({tableName})";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string indexName = reader.GetString("name");
                    bool unique = reader.GetBoolean("unique");

                    // Get the columns for the index
                    List<string> indexColumns = GetIndexColumns(connection, indexName);

                    indices.Add(new IndexSchema
                    {
                        Name = indexName,
                        Unique = unique,
                        Columns = indexColumns
                    });
                }
            }
        }
        return indices;
    }

    private List<string> GetIndexColumns(SqliteConnection connection, string indexName)
    {
        List<string> columns = new List<string>();
        using (var command = connection.CreateCommand())
        {
            command.CommandText = $"PRAGMA index_xinfo({indexName})";  // Use index_xinfo to get column names
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (!reader.IsDBNull(2)) //Column Name is in the third column (2)
                    {
                        columns.Add(reader.GetString(2));
                    }

                }
            }
        }
        return columns;
    }

    // Define the classes to represent the schema
    public class ColumnSchema
    {
        public string? ColumnName { get; set; }
        public string? ColumnType { get; set; }
        public bool NotNull { get; set; }
        public object? DefaultValue { get; set; }  // Use object to handle nulls
        public int PrimaryKey { get; set; }
    }

    public class TableSchema
    {
        public string? Name { get; set; }
        public List<ColumnSchema>? Schema { get; set; }
        public List<ForeignKeySchema>? ForeignKeys { get; set; }
        public List<IndexSchema>? Indices { get; set; }
    }

    public class ForeignKeySchema
    {
        public string? FromColumn { get; set; }
        public string? ToTable { get; set; }
        public string? ToColumn { get; set; }
        public string? OnUpdate { get; set; }
        public string? OnDelete { get; set; }
        public string? Match { get; set; }
    }

    public class IndexSchema
    {
        public string? Name { get; set; }
        public bool Unique { get; set; }
        public List<string>? Columns { get; set; }
    }
}

