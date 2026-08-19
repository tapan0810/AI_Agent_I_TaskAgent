using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Ollama;
using TaskAgent.Plugins;
using TaskAgent.Services;

using TaskAgentClass = TaskAgent.Agents.TaskAgent;

Console.WriteLine("========================================");
Console.WriteLine("         PERSONAL TASK AGENT            ");
Console.WriteLine("========================================");
Console.WriteLine();

var builder = Kernel.CreateBuilder();

builder.AddOllamaChatCompletion(
    modelId: "gemma3",
    endpoint: new Uri("http://localhost:11434")
);

var taskService = new TaskService();

var kernel = builder.Build();

kernel.Plugins.AddFromObject(
    new TaskPlugin(taskService),
    "TaskPlugin"
);

var taskAgent = new TaskAgentClass(kernel);

Console.WriteLine("Semantic Kernel initialized.");
Console.WriteLine("Ollama configured.");
Console.WriteLine("Task plugin registered.");
Console.WriteLine();
Console.WriteLine("Type 'exit' to quit.");
Console.WriteLine();

while (true)
{
    Console.Write("You: ");

    var input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input))
    {
        continue;
    }

    if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Goodbye!");
        break;
    }

    try
    {
        var response = await taskAgent.AskAsync(input);

        Console.WriteLine($"Agent: {response}");
        Console.WriteLine();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        Console.WriteLine();
    }
}