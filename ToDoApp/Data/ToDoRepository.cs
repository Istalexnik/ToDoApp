using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace ToDoApp.Data;

public class ToDoRepository
{
    public ToDoRepository()
    {
        DatabaseConfig.InitializeDatabase();
    }

    public async Task<List<ToDoItem>> GetToDosAsync()
    {
        var todos = new List<ToDoItem>();
        using var connection = new SqliteConnection($"Data Source={DatabaseConfig.DatabasePath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Description, IsComplete, DateCreated FROM ToDoItems";

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            todos.Add(new ToDoItem
            {
                Id = reader.GetInt32(0),
                Description = reader.GetString(1),
                IsComplete = reader.GetBoolean(2),
                DateCreated = DateTime.Parse(reader.GetString(3))
            });
        }
        return todos;
    }

    public async Task AddToDoAsync(ToDoItem todo)
    {
        using var connection = new SqliteConnection($"Data Source={DatabaseConfig.DatabasePath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO ToDoItems (Description, IsComplete, DateCreated) 
            VALUES ($description, $isComplete, $dateCreated)";
        command.Parameters.AddWithValue("$description", todo.Description);
        command.Parameters.AddWithValue("$isComplete", todo.IsComplete ? 1 : 0);
        command.Parameters.AddWithValue("$dateCreated", todo.DateCreated.ToString("yyyy-MM-dd HH:mm:ss"));

        await command.ExecuteNonQueryAsync();
    }

    public async Task UpdateToDoAsync(ToDoItem todo)
    {
        using var connection = new SqliteConnection($"Data Source={DatabaseConfig.DatabasePath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "UPDATE ToDoItems SET Description = $description, IsComplete = $isComplete WHERE Id = $id";
        command.Parameters.AddWithValue("$description", todo.Description);
        command.Parameters.AddWithValue("$isComplete", todo.IsComplete ? 1 : 0);
        command.Parameters.AddWithValue("$id", todo.Id);

        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteToDoAsync(int id)
    {
        using var connection = new SqliteConnection($"Data Source={DatabaseConfig.DatabasePath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM ToDoItems WHERE Id = $id";
        command.Parameters.AddWithValue("$id", id);

        await command.ExecuteNonQueryAsync();
    }
}
