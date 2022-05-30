namespace Beanstalk.Backend.Data;

public interface IPlantRepository
{
    // CREATE
     Task CreatePlantAsync(Plant plant);
    
    // READ
     Task<IEnumerable<Plant?>> GetAllPlantsAsync();
     Task<Plant?> GetPlantAsync(string plantId);
     
     // SEARCH
     Task<IEnumerable<Plant?>> SearchByNameAsync(string searchTerm);
    
    // UPDATE
     Task UpdatePlantAsync(Plant plant);
    
    // DELETE
     Task<bool> DeletePlantAsync(string plantId);
}