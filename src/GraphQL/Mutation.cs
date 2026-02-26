using System.Security.Claims;
using AppAny.HotChocolate.FluentValidation;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.AspNetCore.Authorization;
using Planara.Common.Auth.Claims;
using Planara.Projects.Data;
using Planara.Projects.Data.Domain;
using Planara.Projects.Requests;
using Planara.Projects.Responses;
using Planara.Projects.Validators;

namespace Planara.Projects.GraphQL;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class Mutation
{
    /// <summary>
    /// Запрос на создание нового проекта
    /// </summary>
    /// <param name="request">Данные для создания нового проекта</param>
    /// <param name="claimsPrincipal">Клеймы пользователя</param>
    /// <param name="dataContext">Контекст базы данных</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
    /// <returns>Возвращаяет информацию о созданном проекте</returns>
    [Authorize]
    [GraphQLDescription("Создание нового проекта")]
    public async Task<ProjectResponse> CreateProject(
        [GraphQLDescription("Данные для создания проекта")]
        [UseFluentValidation, UseValidator<CreateProjectRequestValidator>]
        CreateProjectRequest request,
        ClaimsPrincipal claimsPrincipal,
        [Service] DataContext dataContext,
        CancellationToken cancellationToken)
    {
        var userId = claimsPrincipal.GetUserId();

        var project = new Project
        {
            UserId = userId,
            Name = request.Name,
            Description = request.Description,
            IsPrivate = request.IsPrivate
        };
            
        await dataContext.Projects.AddAsync(project, cancellationToken);
        await dataContext.SaveChangesAsync(cancellationToken);

        return new ProjectResponse(project);
    }
}