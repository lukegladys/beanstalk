using Beanstalk.App.Data;
using Beanstalk.App.Endpoints.Internal;
using Beanstalk.App.Models;
using Beanstalk.App.Services;
using FluentValidation;
using FluentValidation.Results;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Redis Configuration
builder.Services.AddSingleton<IConnectionMultiplexer>(opt => 
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")));

builder.Services.AddEndpoints<Program>(builder.Configuration);

builder.Services.AddSingleton<RedisPlantTypeRepository>();
builder.Services.AddSingleton<PlantTypeService>();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Swagger Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseEndpoints<Program>();

// Initialize cached defaults
await app.Services.GetRequiredService<PlantTypeService>().InitializeDataAsync();

app.Run();