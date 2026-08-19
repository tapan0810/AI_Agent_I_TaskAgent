namespace TaskAgent.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool isComplete { get; set; }
    public DateTime CreatedAt { get; set; }
}