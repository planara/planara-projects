using FluentAssertions;
using Planara.Projects.Data.Domain;

namespace Planara.Projects.Tests.Api;

public class QueriesTests : BaseApiTest
{
    public QueriesTests(ApiTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetMyProjects_ReturnsOnlyCurrentUserProjects()
    {
        await DbTestUtils.ResetProjectsDbAsync(Context);

        var anotherUserId = Guid.NewGuid();

        Context.Projects.AddRange(
            new Project
            {
                UserId = UserId,
                Name = "My project",
                Description = "Owner project",
                FileUrl = "https://files.planara.local/my.planara",
                IsPrivate = true,
                UpdatedAt = DateTime.UtcNow
            },
            new Project
            {
                UserId = anotherUserId,
                Name = "Foreign project",
                Description = "Another user project",
                FileUrl = "https://files.planara.local/foreign.planara",
                IsPrivate = true,
                UpdatedAt = DateTime.UtcNow
            }
        );

        await Context.SaveChangesAsync();

        const string query = """
            query MyProjects {
              myProjects(first: 20) {
                totalCount
                nodes {
                  id
                  name
                }
              }
            }
            """;

        using var json = await Client.PostAsync(query);

        json.GetErrors().Should().BeNull();

        var myProjects = json.GetData().GetProperty("myProjects");

        myProjects.GetProperty("totalCount").GetInt32().Should().Be(1);

        var nodes = myProjects.GetProperty("nodes").EnumerateArray().ToArray();

        nodes.Should().HaveCount(1);
        nodes[0].GetProperty("name").GetString().Should().Be("My project");
    }
    
    [Fact]
    public async Task GetProject_ExistingProject_ReturnsProject()
    {
        await DbTestUtils.ResetProjectsDbAsync(Context);

        var project = new Project
        {
            UserId = UserId,
            Name = "My project",
            Description = "Owner project",
            FileUrl = "https://files.planara.local/my.planara",
            IsPrivate = true,
            UpdatedAt = DateTime.UtcNow
        };

        Context.Projects.Add(project);
        await Context.SaveChangesAsync();

        const string query = """
            query GetProject($request: GetProjectByIdRequestInput!) {
              project(request: $request) {
                id
                name
                description
                fileUrl
                isPrivate
              }
            }
            """;

        var variables = new
        {
            request = new
            {
                projectId = project.Id
            }
        };

        using var json = await Client.PostAsync(query, variables);

        json.GetErrors().Should().BeNull();

        var result = json.GetData().GetProperty("project");

        result.GetProperty("id").GetGuid().Should().Be(project.Id);
        result.GetProperty("name").GetString().Should().Be("My project");
        result.GetProperty("description").GetString().Should().Be("Owner project");
        result.GetProperty("fileUrl").GetString().Should().Be("https://files.planara.local/my.planara");
        result.GetProperty("isPrivate").GetBoolean().Should().BeTrue();
    }

    [Fact]
    public async Task GetProject_ForeignProject_ReturnsError()
    {
        await DbTestUtils.ResetProjectsDbAsync(Context);

        var foreignProject = new Project
        {
            UserId = Guid.NewGuid(),
            Name = "Foreign project",
            Description = "Another user project",
            FileUrl = "https://files.planara.local/foreign.planara",
            IsPrivate = true,
            UpdatedAt = DateTime.UtcNow
        };

        Context.Projects.Add(foreignProject);
        await Context.SaveChangesAsync();

        const string query = """
            query GetProject($request: GetProjectByIdRequestInput!) {
              project(request: $request) {
                id
                name
              }
            }
            """;

        var variables = new
        {
            request = new
            {
                projectId = foreignProject.Id
            }
        };

        using var json = await Client.PostAsync(query, variables);

        json.GetErrors().Should().NotBeNull();
    }
}