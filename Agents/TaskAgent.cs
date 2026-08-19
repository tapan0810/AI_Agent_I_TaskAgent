using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;

namespace TaskAgent.Agents;

public class TaskAgent
{
    private readonly ChatCompletionAgent _agent;
    private AgentThread? _thread;

    public TaskAgent(Kernel kernel)
    {
        var settings = new PromptExecutionSettings
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };

        _agent = new ChatCompletionAgent
        {
            Name = "TaskAgent",

            Description = "An AI assistant that manages the user's tasks.",

            Instructions = """
                You are a personal task management assistant.

                You can:
                - Add tasks
                - Show tasks
                - Complete tasks
                - Delete tasks
                - Tell the current date and time

                Always use the available tools when the user asks
                you to perform a task operation.

                Never claim that a task was added unless the
                AddTask tool actually succeeds.

                Never claim that a task was completed unless the
                CompleteTask tool actually succeeds.

                Never claim that a task was deleted unless the
                DeleteTask tool actually succeeds.

                If the user asks to complete or delete a task
                without providing enough information to identify it,
                ask for clarification.

                Keep responses concise and helpful.
                """,

            Kernel = kernel,

            Arguments = new KernelArguments(settings)
        };
    }

    public async Task<string> AskAsync(string userMessage)
    {
        if (string.IsNullOrWhiteSpace(userMessage))
        {
            return "Please enter a message.";
        }

        var message = new ChatMessageContent(
            AuthorRole.User,
            userMessage);

        string finalResponse = string.Empty;

        await foreach (var response in _agent.InvokeAsync(
            message,
            _thread))
        {
            _thread = response.Thread;

            if (response.Message.Content is not null)
            {
                finalResponse = response.Message.Content;
            }
        }

        return finalResponse;
    }
}