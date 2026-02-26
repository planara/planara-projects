using HotChocolate;

namespace Planara.Projects.Requests;

/// <summary>
/// Запрос на создание проекта
/// </summary>
public class CreateProjectRequest
{
    /// <summary>
    /// Название проекта
    /// </summary>
    [GraphQLDescription("Название проекта")]
    public required string Name { get; set; }
    
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
    public bool IsPrivate { get; set; } = true;
}