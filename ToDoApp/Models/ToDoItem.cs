namespace ToDoApp.Models;

public class ToDoItem
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsComplete { get; set; } = false;
    public DateTime DateCreated { get; set; } = DateTime.Now;
}
