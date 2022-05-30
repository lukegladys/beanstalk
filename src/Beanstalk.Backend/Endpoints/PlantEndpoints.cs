using Beanstalk.Backend.Data;
using Beanstalk.Backend.Dtos;
using Beanstalk.Backend.Endpoints.Internal;
using Beanstalk.Backend.Services;
using FluentValidation;
using FluentValidation.Results;
using Mapster;

namespace Beanstalk.Backend.Endpoints;

public class PlantEndpoints : IEndpoint
{
    private const string ContentType = "application/json";
    private const string Tag = "Plants";
    private const string BaseRoute = "plants";
    public static void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IPlantRepository, RedisPlantRepository>();
        services.AddSingleton<PlantService>();
    }

    internal static async Task<IResult> CreatePlantAsync(CreatePlantDto plantDto,
                                                         PlantService plantService,
                                                         IValidator<Plant> validator)
    {
        var plant = plantDto.Adapt<Plant>();
            
        var validationResult = await validator.ValidateAsync(plant);
        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.Errors);
        }

        var created = await plantService.CreatePlantAsync(plant);
        if (!created)
        {
            return Results.BadRequest(new List<ValidationFailure>
            {
                new("PlantId", "A plant with this ID already exists")
            });
        }

        return Results.CreatedAtRoute("GetPlant", new { plantId = plant.PlantId }, plant);
    }
    
    internal static async Task<IResult> GetPlantAsync(string plantId, PlantService plantService)
    {
        var plant = await plantService.GetPlantAsync(plantId);
        return plant is not null ? Results.Ok(plant) : Results.NotFound();
    }
    
    internal static async Task<IResult> GetPlantsAsync(string? searchTerm, PlantService plantService)
    {
        if (searchTerm is not null && !string.IsNullOrWhiteSpace(searchTerm))
        {
            var matchedPlants = await plantService.SearchByNameAsync(searchTerm);
            return Results.Ok(matchedPlants);
        }

        var plants = await plantService.GetAllPlantsAsync();
        return Results.Ok(plants);
    }
    
    internal static async Task<IResult> UpdatePlantAsync(string plantId, Plant plant, PlantService plantService, IValidator<Plant> validator)
    {
        plant.PlantId = plantId;
        var validationResult = await validator.ValidateAsync(plant);
        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.Errors);
        }

        var updated = await plantService.UpdatePlantAsync(plant);
        return updated ? Results.Ok(plant) : Results.NotFound();
    }
    
    internal static async Task<IResult> DeletePlantAsync(string plantId, PlantService plantService)
    {
        var deleted = await plantService.DeletePlantAsync(plantId);
        return deleted ? Results.NoContent() : Results.NotFound();
    }

    public static void DefineEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost(BaseRoute, CreatePlantAsync)
            .WithName("CreatePlant")
            .Accepts<CreatePlantDto>(ContentType)
            .Produces<Plant>(201).Produces<IEnumerable<ValidationFailure>>(400)
            .WithTags(Tag);

        app.MapGet($"{BaseRoute}/{{plantId}}", GetPlantAsync)
            .WithName("GetPlant")
            .Produces<Plant>(200).Produces(404)
            .WithTags(Tag);

        app.MapGet(BaseRoute, GetPlantsAsync)
            .WithName("GetPlants")
            .Produces<IEnumerable<Plant>>(200)
            .WithTags(Tag);

        app.MapPut($"{BaseRoute}/{{plantId}}", UpdatePlantAsync)
            .WithName("UpdatePlant")
            .Accepts<Plant>(ContentType)
            .Produces<Plant>(200).Produces<IEnumerable<ValidationFailure>>(400)
            .WithTags(Tag);

        app.MapDelete($"{BaseRoute}/{{plantId}}", DeletePlantAsync)
            .WithName("DeletePlant")
            .Produces(204).Produces(404)
            .WithTags(Tag);
    }
}