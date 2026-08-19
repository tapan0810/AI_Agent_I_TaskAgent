using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace TaskAgent.Plugins;

public class TaskPlugin
{
    private readonly List<string> _tasks = new();

    // Add Task
    [KernelFunction]
    [Description("Adds a new task to the user's task list.")]
    public string AddTask(
        [Description("The task that should be added.")]
        string task)
    {
        _tasks.Add(task);

        return $"Task added successfully: {task}";
    }

    // Get Tasks
    [KernelFunction]
    [Description("Returns all tasks currently stored in the user's task list.")]
    public string GetTasks()
    {
        if (_tasks.Count == 0)
        {
            return "There are no tasks.";
        }

        return string.Join(
            Environment.NewLine,
            _tasks.Select(
                (task, index) => $"{index + 1}. {task}"
            )
        );
    }

    // Get Current Time
    [KernelFunction]
    [Description("Returns the current date and time.")]
    public string GetCurrentTime()
    {
        return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}