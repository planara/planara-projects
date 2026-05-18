using Microsoft.EntityFrameworkCore;
using Planara.Projects.Data;

namespace Planara.Projects.Tests;

public static class DbTestUtils
{
    public static async Task ResetProjectsDbAsync(
        DataContext db,
        CancellationToken cancellationToken = default)
    {
        await db.Database.ExecuteSqlRawAsync(
            @"TRUNCATE TABLE ""Projects"" RESTART IDENTITY CASCADE;", cancellationToken);
    }
}