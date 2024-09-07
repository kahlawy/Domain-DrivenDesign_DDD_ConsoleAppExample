public class Task
{
    public Guid Id { get; private set; }
    public TaskDescription Description { get; private set; }
    public bool IsCompleted { get; private set; }

    public Task(TaskDescription description)
    {
        Id = Guid.NewGuid();
        Description = description;
        IsCompleted = false;
    }

    public void Complete()
    {
        IsCompleted = true;
    }
}
