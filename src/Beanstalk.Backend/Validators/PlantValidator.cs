using FluentValidation;

namespace Beanstalk.Backend.Validators;

public class PlantValidator : AbstractValidator<Plant>
{
    public PlantValidator()
    {
        RuleFor(plant => plant.PlantId)
            .NotNull()
            .NotEmpty();
        
        RuleFor(plant => plant.Name).NotEmpty();
        
        RuleFor(plant => plant.Type).NotNull();
    }
}