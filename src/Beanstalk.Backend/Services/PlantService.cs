using Beanstalk.Backend.Data;

namespace Beanstalk.Backend.Services;

public class PlantService
{
    private readonly IPlantRepository _plantRepository;

    public PlantService(IPlantRepository plantRepository)
    {
        _plantRepository = plantRepository;
    }

    public async Task<bool> CreatePlantAsync(Plant plant)
    {
        var existingPlant = await GetPlantAsync(plant.PlantId);
        if (existingPlant is not null)
        {
            return false;
        }

        await _plantRepository.CreatePlantAsync(plant);
        return true;
    }
    
    public async Task<IEnumerable<Plant?>> GetAllPlantsAsync()
    {
        return await _plantRepository.GetAllPlantsAsync();
    }
    public async Task<Plant?> GetPlantAsync(string plantId)
    {
        return await _plantRepository.GetPlantAsync(plantId);
    }
    
    public async Task<IEnumerable<Plant?>> SearchByNameAsync(string searchTerm)
    {
        return await _plantRepository.SearchByNameAsync(searchTerm);
    }
    
    public async Task<bool> UpdatePlantAsync(Plant plant)
    {
        var existingPlant = await GetPlantAsync(plant.PlantId);
        if (existingPlant is null)
        {
            return false;
        }

        await _plantRepository.UpdatePlantAsync(plant);
        return true;
    }
    
    public async Task<bool> DeletePlantAsync(string plantId)
    {
        return await _plantRepository.DeletePlantAsync(plantId);
    }
}