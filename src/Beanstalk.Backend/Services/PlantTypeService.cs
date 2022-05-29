using System.Globalization;
using Beanstalk.App.Data;
using Beanstalk.App.Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace Beanstalk.App.Services;

public class PlantTypeService
{
    private readonly RedisPlantTypeRepository _plantTypeRepository;

    public PlantTypeService(RedisPlantTypeRepository plantTypeRepository)
    {
        _plantTypeRepository = plantTypeRepository;
    }
    
    public async Task InitializeDataAsync()
    {
        var exists = await _plantTypeRepository.PlantTypeKeyExistsAsync();

        if (!exists)
        {
            await LoadPlantTypes();
        }
    }

    private async Task LoadPlantTypes()
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            MissingFieldFound = null
        };
        
        using var reader = new StreamReader(@"..\..\data\plantTypeList.csv");
        using var csv = new CsvReader(reader, config);

        await csv.ReadAsync();
        csv.ReadHeader();

        while (await csv.ReadAsync())
        {
            var plantType = csv.GetRecord<PlantType>();
            await _plantTypeRepository.InsertPlantTypeAsync(plantType);
        }
        
    }

    public async Task<PlantType?> GetPlantTypeAsync(string plantTypeId)
    {
        return await _plantTypeRepository.GetPlantTypeAsync(plantTypeId);
    }
    
    public async Task<IEnumerable<PlantType?>> GetAllPlantTypesAsync()
    {
        return await _plantTypeRepository.GetAllPlantTypesAsync();
    }
    
    public async Task<IEnumerable<PlantType?>> SearchByCommonNameAsync(string searchTerm)
    {
        return await _plantTypeRepository.SearchByCommonNameAsync(searchTerm);
    }
}