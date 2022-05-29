using System.Text.Json;
using Beanstalk.App.Models;
using StackExchange.Redis;

namespace Beanstalk.App.Data;

public class RedisPlantRepository : IPlantRepository
{
    private readonly IConnectionMultiplexer _redis;

    public RedisPlantRepository(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task CreatePlantAsync(Plant newPlant)
    {
        var db = _redis.GetDatabase();

        var serialPlant = JsonSerializer.Serialize(newPlant);

        await db.HashSetAsync($"plants", new HashEntry[] 
            {new HashEntry(newPlant.PlantId, serialPlant)});
    }

    public async Task<IEnumerable<Plant?>> GetAllPlantsAsync()
    {
        var db = _redis.GetDatabase();

        var completeSet = await db.HashGetAllAsync("plants");

        if (completeSet.Length == 0) return new List<Plant>();
        
        var obj = Array.ConvertAll(completeSet, val => 
            JsonSerializer.Deserialize<Plant>(val.Value)).ToList();
        return obj;

    }

    public async Task<Plant?> GetPlantAsync(string plantId)
    {
        var db = _redis.GetDatabase();

        var plant = await db.HashGetAsync("plants", plantId);

        return !string.IsNullOrEmpty(plant) ? JsonSerializer.Deserialize<Plant>(plant) : null;
    }

    public async Task<IEnumerable<Plant?>> SearchByNameAsync(string searchTerm)
    {
        var plants = (await GetAllPlantsAsync()).ToList();
        return plants.Any()
            ? plants.Where(plant => plant is not null && plant.Name.Contains(searchTerm))
            : new List<Plant?>();
    }

    public async Task UpdatePlantAsync(Plant updatedPlant)
    {
        var db = _redis.GetDatabase();

        var serialPlant = JsonSerializer.Serialize(updatedPlant);

        await db.HashSetAsync($"plants", new HashEntry[] 
            {new HashEntry(updatedPlant.PlantId, serialPlant)});
    }

    public async Task<bool> DeletePlantAsync(string plantId)
    {
        var db = _redis.GetDatabase();

        return await db.HashDeleteAsync("plants", plantId);
    }
}