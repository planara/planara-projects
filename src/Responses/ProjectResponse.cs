using HotChocolate;
using Planara.Projects.Data.Domain;

namespace Planara.Projects.Responses;

/// <summary>
/// DTO проекта для отправки в качестве ответа на запрос
/// </summary>
[GraphQLDescription("Проект")]
public class ProjectResponse
{
    /// <summary>
    /// ID проекта
    /// </summary>
    [GraphQLDescription("ID проекта")]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Название проекта
    /// </summary>
    [GraphQLDescription("Название проекта")]
    public string Name { get; set; }
    
    /// <summary>
    /// Описание проекта
    /// </summary>
    [GraphQLDescription("Описание проекта")]
    public string? Description { get; set; }

    /// <summary>
    /// Ссылка на файл проекта
    /// </summary>
    [GraphQLDescription("Ссылка на файл проекта")]
    public string? FileUrl { get; set; } = null!;

    /// <summary>
    /// Приватный ли проект
    /// </summary>
    [GraphQLDescription("Приватный ли проект")]
    public bool IsPrivate { get; set; } = true; // фича пока не поддерживается, публичных проектов нет
    
    /// <summary>
    /// Дата создания проекта
    /// </summary>
    [GraphQLDescription("Дата создания проекта")]
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Дата последнего обновления проекта
    /// </summary>
    [GraphQLDescription("Дата последнего обновления проекта")]
    public DateTime? UpdatedAt { get; set; }
    
    public ProjectResponse(Project project)
    {
        Id = project.Id;
        Name = project.Name;
        Description = project.Description;
        FileUrl = project.FileUrl;
        IsPrivate = project.IsPrivate;
        CreatedAt = project.CreatedAt;
        UpdatedAt = project.UpdatedAt;
    }
}