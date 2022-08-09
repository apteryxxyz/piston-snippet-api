using Microsoft.EntityFrameworkCore;
using Backend.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Setup basic middlewares
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

// Setup database context
builder.Services.AddDbContext<SnippetContext>(opt =>
    opt.UseInMemoryDatabase("Backend"));

// Setup the http client for the piston engine API
builder.Services.AddHttpClient("piston", configureClient: client =>
    client.BaseAddress = new Uri(builder.Configuration["PistonUri"]));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseHttpLogging();
app.MapControllers();
app.Run();
