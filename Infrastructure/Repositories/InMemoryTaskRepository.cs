// Infrastructure/Repositories/InMemoryTaskRepository.cs
using System.Collections.Generic;

public class InMemoryTaskRepository : ITaskRepository
{
    private readonly Dictionary<Guid, Task> _tasks = new Dictionary<Guid, Task>();

    public void Add(Task task)
    {
        _tasks.Add(task.Id, task);
    }

    public Task Get(Guid taskId)
    {
        _tasks.TryGetValue(taskId, out var task);
        return task;
    }

    public IEnumerable<Task> GetAll()
    {
        return _tasks.Values;
    }

    public void Save(Task task)
    {
        _tasks[task.Id] = task;
    }
}
