using System.ComponentModel;
using Microsoft.SemanticKernel;
using TaskAgent.Services;

namespace TaskAgent.Plugins;

public class TaskPlugin
{
    private readonly TaskService _taskService;

    public TaskPlugin(TaskService taskService)
    {
        _taskService = taskService;
    }

    [KernelFunction]
    [Description("Adds a new task to the user's task list.")]
    public string AddTask(
        [Description("The title or description of the task.")]
        string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return "The task title cannot be empty.";
        }

        var task = _taskService.AddTask(title);

        return $"Task added successfully. ID: {task.Id}, Title: {task.Title}";
    }

    [KernelFunction]
    [Description("Returns all tasks in the user's task list.")]
    public string GetTasks()
    {
        var tasks = _taskService.GetTasks();

        if (tasks.Count == 0)
        {
            return "There are currently no tasks.";
        }

        return string.Join(
            Environment.NewLine,
            tasks.Select(task =>
                $"ID: {task.Id} | " +
                $"Title: {task.Title} | " +
                $"Completed: {task.IsCompleted} | " +
                $"Created: {task.CreatedAt:yyyy-MM-dd HH:mm:ss}")
        );
    }

    [KernelFunction]
    [Description("Marks a task as completed using its task ID.")]
    public string CompleteTask(
        [Description("The ID of the task to complete.")]
        int taskId)
    {
        var task = _taskService.CompleteTask(taskId);

        if (task == null)
        {
            return $"Task with ID {taskId} was not found.";
        }

        return $"Task {taskId} marked as completed.";
    }

    [KernelFunction]
    [Description("Deletes a task using its task ID.")]
    public string DeleteTask(
        [Description("The ID of the task to delete.")]
        int taskId)
    {
        var deleted = _taskService.DeleteTask(taskId);

        if (!deleted)
        {
            return $"Task with ID {taskId} was not found.";
        }

        return $"Task {taskId} deleted successfully.";
    }

    [KernelFunction]
    [Description("Returns the current local date and time.")]
    public string GetCurrentTime()
    {
        return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}