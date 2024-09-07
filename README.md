
# Domain-Driven Design (DDD) Console Application Example

## Task Management System (Console Application)

This is a simple example of using **Domain-Driven Design (DDD)** to build a task management system in a console application.

### High-Level Design with DDD Concepts

- **Domain**: Task Management
- **Entities**: `Task` (an entity with an ID, description, and status).
- **Value Objects**: `TaskDescription` (a simple value object that represents a task's description).
- **Services**: A domain service for managing the tasks (`TaskService`).
- **Repositories**: A repository for persisting and retrieving tasks (`TaskRepository`).
- **Application Layer**: The console app that interacts with the user and coordinates use cases.

---

### 1. Create the Domain Layer

Start by defining the core domain logic, entities, and value objects.

```csharp
// Domain/Entities/Task.cs
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
```

```csharp
// Domain/ValueObjects/TaskDescription.cs
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
```

---

### 2. Create the Repository Interface

The repository interface abstracts how tasks are stored. In DDD, repositories are part of the domain but their implementation can be placed in the infrastructure layer.

```csharp
// Domain/Repositories/ITaskRepository.cs
public interface ITaskRepository
{
    void Add(Task task);
    Task Get(Guid taskId);
    IEnumerable<Task> GetAll();
    void Save(Task task);
}
```

---

### 3. Create the Domain Service

The service contains the core logic for interacting with the tasks. It works with the repository to manage the state of the tasks.

```csharp
// Domain/Services/TaskService.cs
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
```

---

### 4. Implement the Infrastructure Layer (Repository Implementation)

The repository implementation handles data persistence. For simplicity, let's store tasks in memory.

```csharp
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
```

---

### 5. Create the Application Layer (Console Application)

This is where the user interacts with the system, and the application layer coordinates the use cases by calling the domain services.

```csharp
// Program.cs
using System;

class Program
{
    static void Main(string[] args)
    {
        var taskRepository = new InMemoryTaskRepository();
        var taskService = new TaskService(taskRepository);

        Console.WriteLine("Task Management Console Application");
        
        while (true)
        {
            Console.WriteLine("1. Add Task");
            Console.WriteLine("2. Complete Task");
            Console.WriteLine("3. List Tasks");
            Console.WriteLine("0. Exit");

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.Write("Enter task description: ");
                    var description = Console.ReadLine();
                    taskService.AddTask(description);
                    break;

                case "2":
                    Console.Write("Enter task ID to complete: ");
                    var taskIdInput = Console.ReadLine();
                    if (Guid.TryParse(taskIdInput, out var taskId))
                    {
                        taskService.CompleteTask(taskId);
                    }
                    else
                    {
                        Console.WriteLine("Invalid Task ID");
                    }
                    break;

                case "3":
                    var tasks = taskService.ListTasks();
                    foreach (var task in tasks)
                    {
                        Console.WriteLine($"{task.Id} - {task.Description.Value} - Completed: {task.IsCompleted}");
                    }
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }
        }
    }
}
```

---

## Explanation of the DDD Structure

- **Domain Layer**: Contains the core business logic. The `Task` entity and the `TaskDescription` value object represent key concepts in the domain. The `TaskService` handles operations, enforcing business rules such as task completion.
- **Repository**: The `ITaskRepository` interface defines how tasks are persisted. The `InMemoryTaskRepository` provides a simple in-memory implementation, but it could be replaced with a database implementation without affecting the domain logic.
- **Application Layer**: The console application interacts with the `TaskService`, coordinating the business logic based on user input.

---

## Benefits of Using DDD for Console Apps

Even in a simple console app, applying DDD principles helps:

1. **Separation of Concerns**: By keeping the domain logic separate from the infrastructure and application layers, the code is easier to maintain and extend.
2. **Clear Domain Focus**: The business logic related to tasks is explicitly modeled, making the code easier to understand and modify as the domain evolves.
3. **Testability**: With a clear separation of concerns, testing domain logic becomes easier, as you can unit test the domain services and entities independently of the application or data access layer.

---

## Further Enhancements

1. **Persistence**: Implement a more robust repository, such as a file-based or database repository.
2. **Event-Driven**: Add domain events to signal important business occurrences like task completion.
3. **Modularize the Application**: As the domain grows, consider introducing bounded contexts to separate different areas of responsibility in the system.
