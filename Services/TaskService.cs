using TaskAgent.Models;

namespace TaskAgent.Services;

public class TaskService
{
    private readonly List<TaskItem> _tasks = new();

    private int _nextId = 1;

    public TaskItem AddTask(string title)
    {
        var task = new TaskItem
        {
            Id = _nextId++,
            Title = title,
            IsCompleted = false,
            CreatedAt = DateTime.Now
        };

        _tasks.Add(task);

        return task;
    }

    public IReadOnlyList<TaskItem> GetTasks()
    {
        return _tasks.AsReadOnly();
    }

    public TaskItem? CompleteTask(int id)
    {
        var task = _tasks.FirstOrDefault(x => x.Id == id);

        if (task == null)
        {
            return null;
        }

        task.IsCompleted = true;

        return task;
    }

    public bool DeleteTask(int id)
    {
        var task = _tasks.FirstOrDefault(x => x.Id == id);

        if (task == null)
        {
            return false;
        }

        _tasks.Remove(task);

        return true;
    }
}