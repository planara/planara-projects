using Planara.Common.Database.Domain;

namespace Planara.Projects.Data.Domain;

/// <summary>
/// Проект
/// </summary>
public class Project: BaseEntity
{
    /// <summary>
    /// ID пользователя
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Название проекта
    /// </summary>
    public required string Name { get; set; }
    
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
}