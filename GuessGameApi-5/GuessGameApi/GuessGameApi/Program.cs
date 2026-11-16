var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const int targetNumber = 7;
const string secretKey = "letmein";

app.Run(async (context) =>
{

    if (context.Request.Path != "/guess")
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        return;
    }

    if (!context.Request.Headers.TryGetValue("X-Secret-Key", out var headerValue) || headerValue != secretKey)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsync("Unauthorized: Invalid or missing X-Secret-Key");
        return;
    }

    if (!context.Request.Query.TryGetValue("guess", out var guessQuery) || 
        !int.TryParse(guessQuery, out int userGuess))
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync("Bad Request: Please provide a valid integer in 'guess' parameter");
        return;
    }

    if (userGuess == targetNumber)
    {
        context.Response.StatusCode = StatusCodes.Status200OK;
        await context.Response.WriteAsync("Congratulations! You guessed the number!");
    }
    else
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync("Wrong guess. Try again!");
    }
});

app.Run();
