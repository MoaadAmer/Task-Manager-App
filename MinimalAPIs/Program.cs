using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ITaskService>(new InMemoryTaskService());

var app = builder.Build();


//Use redirect middleware
app.UseRewriter(new RewriteOptions().AddRedirect("tasks/(.*)", "todos/$1"));


app.Use(async (context, next) =>
{
    Console.WriteLine($"[{context.Request.Method} {context.Request.Path} {DateTime.Now}] Started.");
    await next(context);
    Console.WriteLine($"[{context.Request.Method} {context.Request.Path} {DateTime.Now}] Finshed.");
});



var todos = new List<Todo>();

app.MapPost("/todos", (Todo task, ITaskService taskService) =>
{
    taskService.AddTodo(task);
    return TypedResults.Created("/todos/{id}", task);
})
.AddEndpointFilter(async (context, next) =>
    {
        var taskArgument = context.GetArgument<Todo>(0);
        var errors = new Dictionary<string, string[]>();
        if (taskArgument.DueDate < DateTime.UtcNow)
        {
            errors.Add(nameof(taskArgument.DueDate), ["Cannot have due date in the past."]);
        }
        if (taskArgument.isCompleted)
        {
            errors.Add(nameof(taskArgument.isCompleted), ["Cannot add a completed todo."]);
        }
        if (errors.Count > 0)
        {
            return Results.ValidationProblem(errors);
        }

        return await next(context);
    });

app.MapGet("/todos/{id}", Results<Ok<Todo>, NotFound> (int id, ITaskService taskService) =>
{
    var targetTodo = taskService.GetTodoById(id);
    return targetTodo is null ?
    TypedResults.NotFound() :
    TypedResults.Ok(targetTodo);
});

app.MapGet("/todos", (ITaskService taskService) => taskService.GetAllTodos());

app.MapDelete("/todos/{id}", (int id, ITaskService taskService) =>
{

    taskService.DeleteTodoById(id);
    return TypedResults.NoContent();
}
);

app.Run();





public record Todo(int Id, string Name, DateTime DueDate, bool isCompleted);


public interface ITaskService
{
    void AddTodo(Todo task);
    void DeleteTodoById(int id);

    Todo? GetTodoById(int id);
    List<Todo> GetAllTodos();
}


public class InMemoryTaskService : ITaskService
{
    private List<Todo> _todos = [];
    public void AddTodo(Todo task)
    {
        _todos.Add(task);
    }

    public void DeleteTodoById(int id)
    {
        _todos.RemoveAll(t => t.Id == id);
    }

    public List<Todo> GetAllTodos()
    {
        return _todos;
    }

    public Todo? GetTodoById(int id)
    {
        return _todos.SingleOrDefault(t => t.Id == id);
    }
}