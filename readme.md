# TaskAgent — Semantic Kernel + Ollama AI Agent

A small, local, tool-using AI agent built with C#, .NET 9, Microsoft Semantic Kernel, Ollama, and Qwen3.

## Project Overview

TaskAgent is a personal task-management AI agent that understands natural-language requests and can use real C# functions to perform actions.

Example:

    You: Add a task to learn Semantic Kernel

The agent determines that the `AddTask()` tool is required, Semantic Kernel invokes the C# function, the application creates the task, and the result is returned to the AI.

The overall flow is:

    User
      ↓
    TaskAgent
      ↓
    Semantic Kernel
      ↓
    Ollama
      ↓
    Qwen3
      ↓
    Function Selection
      ↓
    TaskPlugin
      ↓
    TaskService
      ↓
    TaskItem
      ↓
    Tool Result
      ↓
    Qwen3
      ↓
    Final Response

## Objectives

This project demonstrates:

- AI Agents
- Semantic Kernel
- Ollama
- Local LLMs
- Qwen3
- Function Calling
- Tool Calling
- Semantic Kernel Plugins
- Native C# Plugins
- Agent Instructions
- Conversation State
- Business Logic Separation
- AI-to-Application integration

## What Is an AI Agent?

A basic LLM application usually looks like:

    User
      ↓
    LLM
      ↓
    Response

An AI agent extends this by giving the model access to tools:

    User
      ↓
    Agent
      ↓
    LLM
      ↓
    Decide which action is required
      ↓
    Tool
      ↓
    Application Logic
      ↓
    Result
      ↓
    LLM
      ↓
    Final Response

For example:

    User:
    "Add a task to learn Semantic Kernel."

    ↓

    Qwen3 determines that AddTask() is required.

    ↓

    Semantic Kernel invokes:

    AddTask("learn Semantic Kernel")

    ↓

    TaskService creates the task.

    ↓

    Tool result is returned to Qwen3.

    ↓

    Qwen3 generates the final response.

## Architecture

    User
      │
      ▼
    Program.cs
      │
      ▼
    TaskAgent
      │
      ▼
    Semantic Kernel
      │
      ▼
    Ollama
      │
      ▼
    Qwen3
      │
      │ Function Selection
      │
      ├───────────────┬────────────────┬────────────────┐
      ▼               ▼                ▼                ▼
    AddTask()      GetTasks()     CompleteTask()    DeleteTask()
      │               │                │                │
      └───────────────┴────────────────┴────────────────┘
                              │
                              ▼
                         TaskPlugin
                              │
                              ▼
                         TaskService
                              │
                              ▼
                           TaskItem

## Project Structure

    TaskAgent/
    │
    ├── Agents/
    │   └── TaskAgent.cs
    │
    ├── Models/
    │   └── TaskItem.cs
    │
    ├── Plugins/
    │   └── TaskPlugin.cs
    │
    ├── Services/
    │   └── TaskService.cs
    │
    ├── Program.cs
    ├── TaskAgent.csproj
    ├── .gitignore
    └── README.md

## Folder Responsibilities

### Agents

`Agents/TaskAgent.cs`

Responsible for:

- Creating the Semantic Kernel `ChatCompletionAgent`
- Agent instructions
- Function-calling configuration
- Conversation state
- Sending user messages to the model
- Processing agent responses

### Models

`Models/TaskItem.cs`

Contains the application's task model.

Properties:

- Id
- Title
- IsCompleted
- CreatedAt

### Plugins

`Plugins/TaskPlugin.cs`

Contains AI-accessible tools.

Available functions:

- AddTask()
- GetTasks()
- CompleteTask()
- DeleteTask()
- GetCurrentTime()

Methods marked with `[KernelFunction]` are exposed to Semantic Kernel.

### Services

`Services/TaskService.cs`

Contains application/business logic.

The plugin delegates operations to the service:

    TaskPlugin
        ↓
    TaskService
        ↓
    TaskItem

This keeps AI-specific code separate from business logic.

## Technologies

| Technology | Purpose |
|---|---|
| C# | Application language |
| .NET 9 | Runtime/framework |
| Semantic Kernel | AI orchestration |
| Semantic Kernel Agents | Agent abstraction |
| Ollama | Local LLM runtime |
| Qwen3 | Tool-capable local LLM |
| Native Plugin | Exposes C# functions to AI |
| In-memory List | Temporary task storage |

## NuGet Packages

Current project packages:

    Microsoft.SemanticKernel                        1.80.0
    Microsoft.SemanticKernel.Core                   1.80.0
    Microsoft.SemanticKernel.Agents.Core            1.80.0
    Microsoft.SemanticKernel.Connectors.Ollama      1.78.0-alpha

Verify installed packages:

    dotnet list package

## Prerequisites

Install:

- .NET 9 SDK
- Ollama
- Qwen3

Check .NET:

    dotnet --version

Check Ollama:

    ollama --version

## Ollama Setup

Install Qwen3:

    ollama pull qwen3

Verify installed models:

    ollama list

Run Qwen3 directly:

    ollama run qwen3

The application uses the default Ollama endpoint:

    http://localhost:11434

## Why Qwen3?

The first version of this project used Gemma 3.

When Semantic Kernel attempted to use function calling, Ollama returned:

    registry.ollama.ai/library/gemma3:latest does not support tools

The problem was that the selected model did not support the tool-calling capability required by the agent in this configuration.

The project therefore uses Qwen3 because the agent requires a model capable of tool/function calling.

This demonstrates an important AI engineering principle:

> The LLM must support the capabilities required by the agent architecture.

## Ollama Configuration

The model is configured in `Program.cs`:

```csharp
builder.AddOllamaChatCompletion(
    modelId: "qwen3",
    endpoint: new Uri("http://localhost:11434")
);