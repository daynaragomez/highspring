using Highspring.Application.Abstractions.Repositories;
using Highspring.Application.Common;
using Highspring.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Highspring.Infrastructure.Persistence.Repositories;

public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException exception)
        {
            if (await TryResolveCartConcurrencyAsync(exception, cancellationToken))
            {
                return await dbContext.SaveChangesAsync(cancellationToken);
            }

            throw new ConcurrencyConflictException($"A concurrent update was detected: {exception.Message}");
        }
    }

    private static async Task<bool> TryResolveCartConcurrencyAsync(
        DbUpdateConcurrencyException exception,
        CancellationToken cancellationToken)
    {
        if (exception.Entries.Count == 0)
        {
            return false;
        }

        if (exception.Entries.Any(entry => entry.Entity is not CartEntity && entry.Entity is not CartItemEntity))
        {
            return false;
        }

        foreach (var entry in exception.Entries)
        {
            var databaseValues = await entry.GetDatabaseValuesAsync(cancellationToken);
            if (databaseValues is null)
            {
                entry.State = EntityState.Detached;
                continue;
            }

            entry.OriginalValues.SetValues(databaseValues);
            entry.CurrentValues.SetValues(databaseValues);
            entry.State = EntityState.Unchanged;
        }

        return true;
    }
}
