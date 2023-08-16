using Microsoft.Extensions.Configuration;
using SantanderCodingTest.Services;

var builder = WebApplication.CreateBuilder(args);

Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddHttpClient<IStoryService, StoryService>(builder.Configuration.GetSection("StoryHttpClient:HttpClientName").Value, client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetSection("StoryHttpClient:BaseUri").Value);
    client.Timeout = new TimeSpan(0, 0, 15);
    client.DefaultRequestHeaders.Clear();
}).SetHandlerLifetime(TimeSpan.FromMinutes(15));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
