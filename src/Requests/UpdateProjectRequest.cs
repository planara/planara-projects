using HotChocolate;

namespace Planara.Projects.Requests;

/// <summary>
/// Запрос на обновление проекта
/// </summary>
[GraphQLDescription("Запрос на обновление проекта")]
public class UpdateProjectRequest
{
    /// <summary>
    /// ID проекта
    /// </summary>
    [GraphQLDescription("ID проекта")]
    public required Guid ProjectId { get; set; }
    
    /// <summary>
    /// Название проекта
    /// </summary>
    [GraphQLDescription("Название проекта")]
    public Optional<string?> Name { get; set; }
    
    /// <summary>
    /// Описание проекта
    /// </summary>
    [GraphQLDescription("Отображаемое имя пользователя")]
    public Optional<string?> Description { get; set; }
    
    /// <summary>
    /// Ссылка на файл проекта
    /// </summary>
    [GraphQLDescription("Ссылка на файл проекта")]
    public Optional<string?> FileUrl { get; set; }
}