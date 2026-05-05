using System.Security.Claims;
using AppAny.HotChocolate.FluentValidation;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Planara.Common.Auth.Claims;
using Planara.Common.Exceptions;
using Planara.Projects.Data;
using Planara.Projects.Requests;
using Planara.Projects.Responses;
using Planara.Projects.Validators;

namespace Planara.Projects.GraphQL;

[ExtendObjectType(OperationTypeNames.Query)]
public class Query
{
    /// <summary>
    /// Получение проекта по ID
    /// </summary>
    /// <param name="dataContext"></param>
    /// <param name="claimsPrincipal"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    [Authorize]
    [GraphQLDescription("Получить проект по Id")]
    public async Task<ProjectResponse> GetProject(
        [Service] DataContext dataContext,
        ClaimsPrincipal claimsPrincipal,
        [UseFluentValidation, UseValidator<GetProjectByIdRequestValidator>]
        GetProjectByIdRequest request,
        CancellationToken cancellationToken)
    {
        var userId = claimsPrincipal.GetUserId();

        var project = await dataContext.Projects
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.Id == request.ProjectId)
            .FirstOrDefaultAsync(cancellationToken);

        if (project is null)
            throw new NotFoundException();
        
        return new ProjectResponse(project);
    }
    
    /// <summary>
    /// Получение списка проектов пользователя
    /// </summary>
    /// <param name="dataContext"></param>
    /// <param name="claimsPrincipal"></param>
    /// <returns></returns>
    [Authorize]
    [UsePaging(MaxPageSize = 50, DefaultPageSize = 20)]
    [UseFiltering]
    [UseSorting]
    [GraphQLDescription("Получить список проектов пользователя")]
    public IQueryable<ProjectResponse> GetMyProjects(
        [Service] DataContext dataContext,
        ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.GetUserId();

        return dataContext.Projects
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(p => new ProjectResponse(p));
    }
}