namespace Beanstalk.Core.Models;

public class PlantType
{
    public string PlantTypeId { get; set; } = $"plant-type:{Guid.NewGuid().ToString()}";
    public string ScientificNameWithAuthor { get; set; } = string.Empty;
    public string CommonName { get; set; } = string.Empty;
    public string Family { get; set; } = string.Empty;
}