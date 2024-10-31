using ToDoApp.Data;
using ToDoApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoApp.Services
{
    public class ToDoService
    {
        private readonly ToDoRepository _toDoRepository;

        public ToDoService(ToDoRepository toDoRepository)
        {
            _toDoRepository = toDoRepository;
        }

        public Task<List<ToDoItem>> GetAllToDosAsync()
        {
            return _toDoRepository.GetToDosAsync();
        }

        public Task AddToDoAsync(string description)
        {
            var newToDo = new ToDoItem { Description = description, IsComplete = false };
            return _toDoRepository.AddToDoAsync(newToDo);
        }

        public Task UpdateToDoAsync(int id, string description, bool isComplete)
        {
            var toDo = new ToDoItem { Id = id, Description = description, IsComplete = isComplete };
            return _toDoRepository.UpdateToDoAsync(toDo);
        }

        public Task DeleteToDoAsync(int id)
        {
            return _toDoRepository.DeleteToDoAsync(id);
        }
    }
}
