using System.Security.Claims;
using AppAny.HotChocolate.FluentValidation;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Planara.Common.Auth.Claims;
using Planara.Common.Exceptions;
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
            FileUrl = request.FileUrl,
            Description = request.Description,
            IsPrivate = request.IsPrivate,
            UpdatedAt = DateTime.UtcNow
        };
            
        await dataContext.Projects.AddAsync(project, cancellationToken);
        await dataContext.SaveChangesAsync(cancellationToken);

        return new ProjectResponse(project);
    }
    
    /// <summary>
    /// Запрос на удаление проекта
    /// </summary>
    /// <param name="request">Данные для удаления проекта</param>
    /// <param name="claimsPrincipal">Клеймы пользователя</param>
    /// <param name="dataContext">Контекст базы данных</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
    /// <returns>Возвращаяет результат операции удаления проекта</returns>
    [Authorize]
    [GraphQLDescription("Удаление проекта")]
    public async Task<DeleteProjectResponse> DeleteProject(
        [GraphQLDescription("Данные для удаления проекта")]
        [UseFluentValidation, UseValidator<DeleteProjectRequestValidator>]
        DeleteProjectRequest request,
        ClaimsPrincipal claimsPrincipal,
        [Service] DataContext dataContext,
        CancellationToken cancellationToken)
    {
        var userId = claimsPrincipal.GetUserId();
            
        var project = await dataContext.Projects
            .SingleOrDefaultAsync(
                x => x.UserId == userId && x.Id == request.ProjectId,
                cancellationToken);
        
        if (project is null)
            return new DeleteProjectResponse { Success = false };
        
        dataContext.Projects.Remove(project);
        await dataContext.SaveChangesAsync(cancellationToken);

        return new DeleteProjectResponse { Success = true };
    }
    
    /// <summary>
    /// Обновление проекта
    /// </summary>
    /// <param name="dataContext">Контекст базы данных</param>
    /// <param name="claimsPrincipal">Клеймы пользователя, для получения ID</param>
    /// <param name="request">Запрос на обновление проекта</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
    /// <exception cref="NotFoundException">Исключение, если проект не был найден</exception>
    [Authorize]
    [GraphQLDescription("Обновление профиля пользователя")]
    public async Task<ProjectResponse> UpdateProject(
        [Service] DataContext dataContext,
        ClaimsPrincipal claimsPrincipal,
        [GraphQLDescription("Данные для обновления проекта")]
        [UseFluentValidation, UseValidator<UpdateProjectRequestValidator>]
        UpdateProjectRequest request,
        CancellationToken cancellationToken
    )
    {
        var userId = claimsPrincipal.GetUserId();

        var project = await dataContext.Projects
            .SingleOrDefaultAsync(
                x => x.UserId == userId && x.Id == request.ProjectId,
                cancellationToken);

        if (project is null)
            throw new NotFoundException();
        
        // Обновление проекта пользователя, только если пришли данные
        Patch(request.Name, project.Name, v => project.Name = v);
        Patch(request.Description, project.Description, v => project.Description = v);
        Patch(request.FileUrl, project.FileUrl, v => project.FileUrl = v);
        
        project.UpdatedAt = DateTime.UtcNow;
        
        await dataContext.SaveChangesAsync(cancellationToken);
        
        return new ProjectResponse(project);
    }
    
    /// <summary>
    /// Обновление полей сущности только при необходмости (передали новое значение)
    /// </summary>
    private static void Patch(Optional<string?> opt, string? oldValue, Action<string> set)
    {
        if (opt.HasValue && opt.Value is not null && !String.Equals(opt.Value, oldValue, StringComparison.Ordinal))
            set(opt.Value);
    }
}