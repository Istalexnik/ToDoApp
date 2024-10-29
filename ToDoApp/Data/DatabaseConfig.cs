using System.IO;
using Microsoft.Maui.Storage;

namespace ToDoApp.Data;

public static class DatabaseConfig
{
    public static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, "ToDoApp.db");

    public static void InitializeDatabase()
    {
        using var connection = new Microsoft.Data.Sqlite.SqliteConnection($"Data Source={DatabasePath}");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS ToDoItems (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Description TEXT NOT NULL,
                IsComplete INTEGER NOT NULL,
                DateCreated TEXT NOT NULL
            );";
        command.ExecuteNonQuery();
    }
}
