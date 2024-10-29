using ToDoApp.Data;
using ToDoApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoApp.Services;

public class ToDoService
{
    private readonly ToDoRepository _todoRepository;

    public ToDoService(ToDoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public Task<List<ToDoItem>> GetAllToDosAsync()
    {
        return _todoRepository.GetToDosAsync();
    }

    public Task AddToDoAsync(string description)
    {
        var todo = new ToDoItem { Description = description, IsComplete = false };
        return _todoRepository.AddToDoAsync(todo);
    }

    public Task UpdateToDoAsync(int id, string description, bool isComplete)
    {
        var todo = new ToDoItem { Id = id, Description = description, IsComplete = isComplete };
        return _todoRepository.UpdateToDoAsync(todo);
    }

    public Task DeleteToDoAsync(int id)
    {
        return _todoRepository.DeleteToDoAsync(id);
    }
}
