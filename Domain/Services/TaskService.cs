public class TaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public void AddTask(string description)
    {
        var task = new Task(new TaskDescription(description));
        _taskRepository.Add(task);
    }

    public void CompleteTask(Guid taskId)
    {
        var task = _taskRepository.Get(taskId);
        if (task != null)
        {
            task.Complete();
            _taskRepository.Save(task);
        }
    }

    public IEnumerable<Task> ListTasks()
    {
        return _taskRepository.GetAll();
    }
}
