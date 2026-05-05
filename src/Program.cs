using System.Reflection;
using AppAny.HotChocolate.FluentValidation;
using HotChocolate.Types;
using Planara.Common.Auth.Jwt;
using Planara.Common.Configuration;
using Planara.Common.Database;
using Planara.Common.GraphQL;
using Planara.Common.GraphQL.Filters;
using Planara.Common.GraphQL.Fusion;
using Planara.Common.Host;
using Planara.Common.Validators;
using Planara.Projects.Data;
using Planara.Projects.GraphQL;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.AddSettingsJson();
builder.Services
    .AddValidators(Assembly.GetExecutingAssembly())
    .AddJwtAuth(builder.Configuration)
    // .AddCors()
    .AddLogging();

builder.Services
    .AddRouting()
    .AddGraphQLServer()
    .AddFiltering()
    .AddSorting()
    .AddErrorFilter<ErrorFilter>()
    .AddQueryType(m => m.Name(OperationTypeNames.Query))
    .AddType<Query>()
    .AddMutationType(m => m.Name(OperationTypeNames.Mutation))
    .AddType<Mutation>()
    .AddAuthorization() 
    .AddFluentValidation(options =>
    {
        options.UseInputValidators();
        options.UseDefaultErrorMapper();
    })
    .ModifyRequestOptions(o => o.IncludeExceptionDetails = builder.Environment.IsDevelopment())
    .PublishSchemaToRedis(
        _ =>
            ConnectionMultiplexer.Connect(
                builder.Configuration.GetValue<string>("DbConnections:Redis:ConnectionString")!,
                c => c.CertificateValidation += (_, _, _, _) => true
            ),
        builder.Configuration.GetValue<string>("GraphQL:Name")!,
        WellKnownSchema.Projects
    )
    .InitializeOnStartup();

builder.Services.AddDataContext<DataContext>(
    builder.Configuration.GetValue<string>("DbConnections:Postgres:ConnectionString")!,
    builder.Configuration.GetValue<int>("DbConnections:Postgres:MaxRetry"),
    builder.Configuration.GetValue<int>("DbConnections:Postgres:MaxDelaySec")
);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();

app.PrepareAndRun<DataContext>(args);