using System.ComponentModel.DataAnnotations;
using Beanstalk.Core.Models;

namespace Beanstalk.Backend.Dtos;

public class CreatePlantDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public PlantType Type { get; set; } = null!;
}