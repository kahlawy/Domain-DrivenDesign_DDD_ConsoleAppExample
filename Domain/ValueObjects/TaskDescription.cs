
public class TaskDescription
{
    public string Value { get; private set; }

    public TaskDescription(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Task description cannot be empty");
        }
        Value = value;
    }
}
