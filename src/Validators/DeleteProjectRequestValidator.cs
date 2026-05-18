using FluentValidation;
using Planara.Projects.Requests;

namespace Planara.Projects.Validators;

public class DeleteProjectRequestValidator: AbstractValidator<DeleteProjectRequest>
{
    public DeleteProjectRequestValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithMessage("ID проекта явлется обязательным");
    }
}