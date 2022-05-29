using Beanstalk.App.Data;
using Beanstalk.App.Endpoints.Internal;
using Beanstalk.App.Models;
using Beanstalk.App.Services;
using FluentValidation;
using FluentValidation.Results;

namespace Beanstalk.App.Endpoints;

public class PlantTypeEndpoints : IEndpoint
{
    private const string ContentType = "application/json";
    private const string Tag = "PlantTypes";
    private const string BaseRoute = "plant-types";
    public static void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<RedisPlantTypeRepository>();
        services.AddSingleton<PlantTypeService>();
    }
    
    internal static async Task<IResult> GetPlantTypeAsync(string plantTypeId, PlantTypeService plantTypeService)
    {
        var plantType = await plantTypeService.GetPlantTypeAsync(plantTypeId);
        return plantType is not null ? Results.Ok(plantType) : Results.NotFound();
    }
    
    internal static async Task<IResult> GetPlantTypesAsync(string? searchTerm, PlantTypeService plantTypeService)
    {
        if (searchTerm is not null && !string.IsNullOrWhiteSpace(searchTerm))
        {
            var matchedPlants = await plantTypeService.SearchByCommonNameAsync(searchTerm);
            return Results.Ok(matchedPlants);
        }

        var plants = await plantTypeService.GetAllPlantTypesAsync();
        return Results.Ok(plants);
    }

    public static void DefineEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet($"{BaseRoute}/{{plantTypeId}}", GetPlantTypeAsync)
            .WithName("GetPlantType")
            .Produces<Plant>(200).Produces(404)
            .WithTags(Tag);

        app.MapGet(BaseRoute, GetPlantTypesAsync)
            .WithName("GetPlantTypes")
            .Produces<IEnumerable<Plant>>(200)
            .WithTags(Tag);
    }
}