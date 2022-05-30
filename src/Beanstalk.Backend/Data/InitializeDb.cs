using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Beanstalk.Backend.Data;

public static class InitializeDb
{
    public static async Task PrepPopulation(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        { 
            await SeedData(serviceScope.ServiceProvider.GetService<RedisPlantTypeRepository>());
        }
    }
        
    private static async Task SeedData(RedisPlantTypeRepository repo)
    {
        Console.WriteLine("Seeding new platforms...");

        var exists = await repo.PlantTypeKeyExistsAsync();

        if (!exists)
        {
            await LoadPlantTypes(repo);
        }
    }
    
    private static async Task LoadPlantTypes(RedisPlantTypeRepository repo)
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
            await repo.InsertPlantTypeAsync(plantType);
        }
        
    }
}