using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ToDoApp.Models;
using Microsoft.Maui.Storage;

namespace ToDoApp.Services;

public class ToDoListService
{
    private readonly string dbPath;

    public ToDoListService()
    {
        dbPath = Path.Combine(FileSystem.AppDataDirectory, "ToDoList.db");
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using var connection = new SqliteConnection($"Data Source={dbPath}");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS ToDoItems (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Description TEXT NOT NULL,
                IsComplete INTEGER NOT NULL
            );";
        command.ExecuteNonQuery();
    }

    public async Task<List<ToDoItem>> GetTasksAsync()
    {
        var tasks = new List<ToDoItem>();

        using var connection = new SqliteConnection($"Data Source={dbPath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM ToDoItems";

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            tasks.Add(new ToDoItem
            {
                Id = reader.GetInt32(0),
                Description = reader.GetString(1),
                IsComplete = reader.GetInt32(2) == 1
            });
        }

        return tasks;
    }

    public async Task AddTaskAsync(string description)
    {
        using var connection = new SqliteConnection($"Data Source={dbPath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO ToDoItems (Description, IsComplete) 
            VALUES ($description, $isComplete)";
        command.Parameters.AddWithValue("$description", description);
        command.Parameters.AddWithValue("$isComplete", 0);

        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteTaskAsync(int id)
    {
        using var connection = new SqliteConnection($"Data Source={dbPath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM ToDoItems WHERE Id = $id";
        command.Parameters.AddWithValue("$id", id);

        await command.ExecuteNonQueryAsync();
    }

    public async Task MarkTaskAsCompleteAsync(int id)
    {
        using var connection = new SqliteConnection($"Data Source={dbPath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "UPDATE ToDoItems SET IsComplete = 1 WHERE Id = $id";
        command.Parameters.AddWithValue("$id", id);

        await command.ExecuteNonQueryAsync();
    }

    public async Task MarkTaskAsIncompleteAsync(int id)
    {
        using var connection = new SqliteConnection($"Data Source={dbPath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "UPDATE ToDoItems SET IsComplete = 0 WHERE Id = $id";
        command.Parameters.AddWithValue("$id", id);

        await command.ExecuteNonQueryAsync();
    }

    public async Task UpdateTaskDescriptionAsync(int id, string newDescription)
    {
        using var connection = new SqliteConnection($"Data Source={dbPath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "UPDATE ToDoItems SET Description = $description WHERE Id = $id";
        command.Parameters.AddWithValue("$description", newDescription);
        command.Parameters.AddWithValue("$id", id);

        await command.ExecuteNonQueryAsync();
    }

}
