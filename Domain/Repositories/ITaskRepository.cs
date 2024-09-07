public interface ITaskRepository
{
    void Add(Task task);
    Task Get(Guid taskId);
    IEnumerable<Task> GetAll();
    void Save(Task task);
}
