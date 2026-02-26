using FluentValidation;
using Planara.Projects.Requests;

namespace Planara.Projects.Validators;

public class CreateProjectRequestValidator: AbstractValidator<CreateProjectRequest>
{
    public CreateProjectRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название проекта обязательно.")
            .MinimumLength(2).WithMessage("Название проекта должно быть минимум 2 символа.")
            .MaximumLength(100).WithMessage("Название проекта должно быть максимум 100 символов.");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Описание проекта должно быть максимум 2000 символов.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.FileUrl)
            .Must(BeValidHttpUrl).WithMessage("Ссылка на файл проекта должна быть корректным URL (http/https).")
            .When(x => !string.IsNullOrWhiteSpace(x.FileUrl));
    }

    private static bool BeValidHttpUrl(string? url)
        => Uri.TryCreate(url, UriKind.Absolute, out var u)
           && (u.Scheme == Uri.UriSchemeHttp || u.Scheme == Uri.UriSchemeHttps);
}