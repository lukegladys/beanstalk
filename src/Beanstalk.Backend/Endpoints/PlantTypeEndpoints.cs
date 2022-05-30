using Beanstalk.Backend.Data;
using Beanstalk.Backend.Endpoints.Internal;
using Beanstalk.Backend.Services;
using FluentValidation;
using FluentValidation.Results;

namespace Beanstalk.Backend.Endpoints;

public class PlantTypeEndpoints : IEndpoint
{
    private const string ContentType = "application/json";
    private const string Tag = "PlantTypes";
    private const string BaseRoute = "plant-types";
    public static void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<RedisPlantTypeRepository>();
    }
    
    internal static async Task<IResult> GetPlantTypeAsync(string plantTypeId, RedisPlantTypeRepository plantTypeRepository)
    {
        var plantType = await plantTypeRepository.GetPlantTypeAsync(plantTypeId);
        return plantType is not null ? Results.Ok(plantType) : Results.NotFound();
    }
    
    internal static async Task<IResult> GetPlantTypesAsync(string? searchTerm,
        RedisPlantTypeRepository plantTypeRepository,
        int pageSize=50,
        int pageOffset=0)
    {
        if (searchTerm is not null && !string.IsNullOrWhiteSpace(searchTerm))
        {
            var matchedPlants = await plantTypeRepository.SearchByCommonNameAsync(searchTerm);
            return Results.Ok(matchedPlants);
        }

        var plants = await plantTypeRepository.GetPlantTypesAsync(pageSize, pageOffset);
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