using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MB.Outbox.Persistence.EfCore;

public class EfCoreStorageBuilder<TDbContext> where TDbContext : DbContext
{
    public EfCoreStorageBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public IServiceCollection Services { get; }
}