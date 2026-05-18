using HotChocolate;

namespace Planara.Projects.Responses;

/// <summary>
/// Результат операции удаления проекта
/// </summary>
[GraphQLDescription("Результат операции удаления проекта")]
public class DeleteProjectResponse
{
    /// <summary>
    /// Признак успешного выполнения операции
    /// </summary>
    [GraphQLDescription("Признак успешного выполнения операции")]
    public bool Success { get; set; }
}
