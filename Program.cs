using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Ollama;
using TaskAgent.Plugins;

var builder = Kernel.CreateBuilder();

builder.AddOllamaChatCompletion(
    modelId: "gemma3",
    endpoint: new Uri("http://localhost:11434")
);

builder.Plugins.AddFromType<TaskPlugin>();

var kernel = builder.Build();

Console.WriteLine("Semantic Kernel initialized!");
Console.WriteLine("Plugin registered successfully!");