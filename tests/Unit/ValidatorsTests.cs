using FluentAssertions;
using HotChocolate;
using Planara.Projects.Requests;
using Planara.Projects.Validators;

namespace Planara.Projects.Tests.Unit;

public class ValidatorsTests
{
    [Fact]
    public void CreateProject_ValidRequest_Succeeds()
    {
        var validator = new CreateProjectRequestValidator();

        var request = new CreateProjectRequest
        {
            Name = "Demo project",
            Description = "Description",
            FileUrl = "https://files.planara.local/demo.planara",
            IsPrivate = true
        };

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void UpdateProject_NameNotProvided_Succeeds()
    {
        var validator = new UpdateProjectRequestValidator();

        var request = new UpdateProjectRequest
        {
            ProjectId = Guid.NewGuid(),
            Name = default,
            Description = default,
            FileUrl = default
        };

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void UpdateProject_NameProvidedEmpty_Fails()
    {
        var validator = new UpdateProjectRequestValidator();

        var request = new UpdateProjectRequest
        {
            ProjectId = Guid.NewGuid(),
            Name = new Optional<string?>("")
        };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void DeleteProject_EmptyProjectId_Fails()
    {
        var validator = new DeleteProjectRequestValidator();

        var request = new DeleteProjectRequest
        {
            ProjectId = Guid.Empty
        };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void GetProjectById_EmptyProjectId_Fails()
    {
        var validator = new GetProjectByIdRequestValidator();

        var request = new GetProjectByIdRequest
        {
            ProjectId = Guid.Empty
        };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }
    
    [Fact]
    public void CreateProject_HttpFileUrl_Succeeds()
    {
        var validator = new CreateProjectRequestValidator();

        var request = new CreateProjectRequest
        {
            Name = "Test project",
            Description = "Description",
            FileUrl = "http://files.planara.local/project.planara",
            IsPrivate = true
        };

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void CreateProject_HttpsFileUrl_Succeeds()
    {
        var validator = new CreateProjectRequestValidator();

        var request = new CreateProjectRequest
        {
            Name = "Test project",
            Description = "Description",
            FileUrl = "https://files.planara.local/project.planara",
            IsPrivate = true
        };

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void CreateProject_FtpFileUrl_Fails()
    {
        var validator = new CreateProjectRequestValidator();

        var request = new CreateProjectRequest
        {
            Name = "Test project",
            Description = "Description",
            FileUrl = "ftp://files.planara.local/project.planara",
            IsPrivate = true
        };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.ErrorMessage.Contains("URL", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void CreateProject_InvalidFileUrl_Fails()
    {
        var validator = new CreateProjectRequestValidator();

        var request = new CreateProjectRequest
        {
            Name = "Test project",
            Description = "Description",
            FileUrl = "not-url",
            IsPrivate = true
        };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.ErrorMessage.Contains("URL", StringComparison.OrdinalIgnoreCase));
    }
    
    [Fact]
    public void UpdateProject_NameProvidedNull_Succeeds()
    {
        var validator = new UpdateProjectRequestValidator();

        var request = new UpdateProjectRequest
        {
            ProjectId = Guid.NewGuid(),
            Name = new Optional<string?>(null)
        };

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void UpdateProject_NameProvidedWhitespace_Fails()
    {
        var validator = new UpdateProjectRequestValidator();

        var request = new UpdateProjectRequest
        {
            ProjectId = Guid.NewGuid(),
            Name = new Optional<string?>("   ")
        };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.ErrorMessage.Contains("Название проекта не может быть пустым", StringComparison.OrdinalIgnoreCase));
    }
}