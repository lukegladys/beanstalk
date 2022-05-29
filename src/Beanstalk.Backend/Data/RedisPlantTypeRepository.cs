using System.Text.Json;
using Beanstalk.App.Models;
using StackExchange.Redis;

namespace Beanstalk.App.Data;

public class RedisPlantTypeRepository
{
    private readonly IConnectionMultiplexer _redis;

    public RedisPlantTypeRepository(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task InsertPlantTypeAsync(PlantType plantType)
    {
        var db = _redis.GetDatabase();

        var serialPlantType = JsonSerializer.Serialize(plantType);

        await db.HashSetAsync($"plant-types", new HashEntry[] 
            {new HashEntry(plantType.PlantTypeId, serialPlantType)});
    }
    
    public async Task<bool> PlantTypeKeyExistsAsync()
    {
        var db = _redis.GetDatabase();
        return await db.KeyExistsAsync("plant-types");
    }

    public async Task<IEnumerable<PlantType?>> GetAllPlantTypesAsync()
    {
        var db = _redis.GetDatabase();

        var completeSet = await db.HashGetAllAsync("plant-types");

        if (completeSet.Length == 0) return new List<PlantType>();
        
        var obj = Array.ConvertAll(completeSet, val => 
            JsonSerializer.Deserialize<PlantType>(val.Value)).ToList();
        return obj;

    }

    public async Task<PlantType?> GetPlantTypeAsync(string plantId)
    {
        var db = _redis.GetDatabase();

        var plant = await db.HashGetAsync("plant-types", plantId);

        return !string.IsNullOrEmpty(plant) ? JsonSerializer.Deserialize<PlantType>(plant) : null;
    }

    public async Task<IEnumerable<PlantType?>> SearchByCommonNameAsync(string searchTerm)
    {
        var plantTypes = (await GetAllPlantTypesAsync()).ToList();
        return plantTypes.Any()
            ? plantTypes.Where(plantType => plantType is not null && plantType.CommmonName.Contains(searchTerm))
            : new List<PlantType?>();
    }
}