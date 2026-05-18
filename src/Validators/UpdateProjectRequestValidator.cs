using FluentValidation;
using HotChocolate;
using Planara.Projects.Requests;

namespace Planara.Projects.Validators;

public class UpdateProjectRequestValidator: AbstractValidator<UpdateProjectRequest>
{
    public UpdateProjectRequestValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithMessage("ID проекта явлется обязательным");
        
        RuleFor(x => x.Name)
            .Must(BeNonEmptyWhenProvided)
            .WithMessage("Название проекта не может быть пустым.");
    }
    
    private static bool BeNonEmptyWhenProvided(Optional<string?> opt)
    {
        if (!opt.HasValue) return true;
        
        if (opt.Value is null) return true;
        
        return !string.IsNullOrWhiteSpace(opt.Value);
    }
}