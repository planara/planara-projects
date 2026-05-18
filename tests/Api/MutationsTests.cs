using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Planara.Projects.Data.Domain;

namespace Planara.Projects.Tests.Api;

public class MutationsTests : BaseApiTest
{
    public MutationsTests(ApiTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task CreateProject_ValidRequest_CreatesProject()
    {
        await DbTestUtils.ResetProjectsDbAsync(Context);

        const string mutation = """
            mutation CreateProject($request: CreateProjectRequestInput!) {
              createProject(request: $request) {
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
                name = "Demo project",
                description = "Test description",
                fileUrl = "https://files.planara.local/demo.planara",
                isPrivate = true
            }
        };

        using var json = await Client.PostAsync(mutation, variables);

        json.GetErrors().Should().BeNull();

        var project = json.GetData().GetProperty("createProject");

        project.GetProperty("name").GetString().Should().Be("Demo project");
        project.GetProperty("description").GetString().Should().Be("Test description");
        project.GetProperty("fileUrl").GetString().Should().Be("https://files.planara.local/demo.planara");
        project.GetProperty("isPrivate").GetBoolean().Should().BeTrue();

        var count = await Context.Projects.CountAsync();
        count.Should().Be(1);
    }

    [Fact]
    public async Task UpdateProject_PartialRequest_UpdatesOnlyProvidedFields()
    {
        await DbTestUtils.ResetProjectsDbAsync(Context);

        var project = new Project
        {
            UserId = UserId,
            Name = "Old name",
            Description = "Old description",
            FileUrl = "https://files.planara.local/old.planara",
            IsPrivate = true,
            UpdatedAt = DateTime.UtcNow
        };

        Context.Projects.Add(project);
        await Context.SaveChangesAsync();

        const string mutation = """
            mutation UpdateProject($request: UpdateProjectRequestInput!) {
              updateProject(request: $request) {
                id
                name
                description
                fileUrl
              }
            }
            """;

        var variables = new
        {
            request = new
            {
                projectId = project.Id,
                name = "New name"
            }
        };

        using var json = await Client.PostAsync(mutation, variables);

        json.GetErrors().Should().BeNull();

        var updated = json.GetData().GetProperty("updateProject");

        updated.GetProperty("name").GetString().Should().Be("New name");
        updated.GetProperty("description").GetString().Should().Be("Old description");
        updated.GetProperty("fileUrl").GetString().Should().Be("https://files.planara.local/old.planara");
    }

    [Fact]
    public async Task DeleteProject_ExistingProject_RemovesProject()
    {
        await DbTestUtils.ResetProjectsDbAsync(Context);

        var project = new Project
        {
            UserId = UserId,
            Name = "Project to delete",
            Description = "Description",
            FileUrl = "https://files.planara.local/delete.planara",
            IsPrivate = true,
            UpdatedAt = DateTime.UtcNow
        };

        Context.Projects.Add(project);
        await Context.SaveChangesAsync();

        const string mutation = """
            mutation DeleteProject($request: DeleteProjectRequestInput!) {
              deleteProject(request: $request) {
                success
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

        using var json = await Client.PostAsync(mutation, variables);

        json.GetErrors().Should().BeNull();

        var result = json.GetData().GetProperty("deleteProject");

        result.GetProperty("success").GetBoolean().Should().BeTrue();

        var exists = await Context.Projects.AnyAsync(x => x.Id == project.Id);
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteProject_ForeignProject_ReturnsFalse()
    {
        await DbTestUtils.ResetProjectsDbAsync(Context);

        var foreignProject = new Project
        {
            UserId = Guid.NewGuid(),
            Name = "Foreign project",
            Description = "Description",
            FileUrl = "https://files.planara.local/foreign.planara",
            IsPrivate = true,
            UpdatedAt = DateTime.UtcNow
        };

        Context.Projects.Add(foreignProject);
        await Context.SaveChangesAsync();

        const string mutation = """
            mutation DeleteProject($request: DeleteProjectRequestInput!) {
              deleteProject(request: $request) {
                success
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

        using var json = await Client.PostAsync(mutation, variables);

        json.GetErrors().Should().BeNull();

        var result = json.GetData().GetProperty("deleteProject");

        result.GetProperty("success").GetBoolean().Should().BeFalse();

        var exists = await Context.Projects.AnyAsync(x => x.Id == foreignProject.Id);
        exists.Should().BeTrue();
    }
    
    [Fact]
    public async Task UpdateProject_ForeignProject_ReturnsError()
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

        const string mutation = """
                                mutation UpdateProject($request: UpdateProjectRequestInput!) {
                                  updateProject(request: $request) {
                                    id
                                    name
                                  }
                                }
                                """;

        var variables = new
        {
            request = new
            {
                projectId = foreignProject.Id,
                name = "Updated name"
            }
        };

        using var json = await Client.PostAsync(mutation, variables);

        json.GetErrors().Should().NotBeNull();
    }
}