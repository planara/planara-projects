using HotChocolate;

namespace Planara.Projects.Requests;

/// <summary>
/// Запрос на удаление проекта
/// </summary>
[GraphQLDescription("Запрос на удаление проекта")]
public class DeleteProjectRequest
{
    /// <summary>
    /// ID проекта
    /// </summary>
    [GraphQLDescription("ID проекта")]
    public required Guid ProjectId { get; set; }
}
