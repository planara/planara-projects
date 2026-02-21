using FluentValidation;
using Planara.Projects.Requests;

namespace Planara.Projects.Validators;

public class GetProjectByIdRequestValidator: AbstractValidator<GetProjectByIdRequest>
{
    public GetProjectByIdRequestValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithMessage("ID проекта явлется обязательным");
    }
}