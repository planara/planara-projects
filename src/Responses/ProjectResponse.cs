using Planara.Projects.Data.Domain;

namespace Planara.Projects.Responses;

/// <summary>
/// DTO проекта для отправки в качестве ответа на запрос
/// </summary>
public class ProjectResponse
{
    /// <summary>
    /// ID проекта
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Название проекта
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Описание проекта
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Ссылка на файл проекта
    /// </summary>
    public string? FileUrl { get; set; } = null!;

    /// <summary>
    /// Приватный ли проект
    /// </summary>
    public bool IsPrivate { get; set; } = true;
    
    /// <summary>
    /// Дата создания проекта
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Дата последнего обновления проекта
    /// </summary>
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