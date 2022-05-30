using Beanstalk.Backend.Data;
using Beanstalk.Backend.Endpoints.Internal;
using Beanstalk.Backend.Services;
using FluentValidation;
using FluentValidation.Results;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Redis Configuration
builder.Services.AddSingleton<IConnectionMultiplexer>(opt => 
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")));

builder.Services.AddEndpoints<Program>(builder.Configuration);

builder.Services.AddSingleton<RedisPlantTypeRepository>();

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
await InitializeDb.PrepPopulation(app);

app.Run();