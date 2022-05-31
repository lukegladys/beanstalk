using System.Text.Json;

namespace Beanstalk.Frontend.Data;

public class PlantServiceClient
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<PlantServiceClient> _logger;

    public PlantServiceClient(IConfiguration configuration,
        IHttpClientFactory httpClientFactory,
        ILogger<PlantServiceClient> logger)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<List<Plant>> GetPlantsAsync()
    {
        // Create the client
        var client = _httpClientFactory.CreateClient();
        
        var resultMessage =
            await client.GetFromJsonAsync<IEnumerable<Plant>>(
                $"{_configuration.GetConnectionString("BackendBase")}/plants");

        return resultMessage is not null ? resultMessage.ToList() : new List<Plant>();
    }
}