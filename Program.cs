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
