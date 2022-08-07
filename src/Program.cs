using Microsoft.EntityFrameworkCore;
using Backend.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Setup middlewares

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

builder.Services.AddDbContext<SnippetContext>(opt =>
    opt.UseInMemoryDatabase("Api"));
builder.Services.AddHttpClient("piston", configureClient: client =>
    client.BaseAddress = new Uri("https://emkc.org/api/v2/piston/"));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseHttpLogging();
app.MapControllers();
app.Run();

public partial class Program { }