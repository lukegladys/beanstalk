namespace Beanstalk.Frontend.Data;

public class PlantServiceClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<PlantServiceClient> _logger;

    public PlantServiceClient(IHttpClientFactory httpClientFactory, ILogger<PlantServiceClient> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public Task<IEnumerable<Plant>> GetPlantsAsync()
    {
        // Create the client
        HttpClient client = _httpClientFactory.CreateClient();
    }
}