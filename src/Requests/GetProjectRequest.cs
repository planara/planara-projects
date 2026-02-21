namespace Planara.Projects.Requests;

/// <summary>
/// Запрос на получение проекта по ID
/// </summary>
public class GetProjectByIdRequest
{
    /// <summary>
    /// ID проекта
    /// </summary>
    public required Guid ProjectId { get; set; }
}