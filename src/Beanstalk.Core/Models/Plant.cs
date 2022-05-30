using System.ComponentModel.DataAnnotations;

namespace Beanstalk.Core.Models;

public class Plant
{
    [Required]
    public string PlantId { get; set; } = $"plant:{Guid.NewGuid().ToString()}";
    
    [Required]
    public string Name { get; set; } = string.Empty;

    public PlantType? Type { get; set; } = new();
}