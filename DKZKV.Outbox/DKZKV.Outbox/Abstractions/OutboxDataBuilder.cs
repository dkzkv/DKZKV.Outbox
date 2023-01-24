using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace DKZKV.Outbox.Abstractions;

public class OutboxDataBuilder
{
    public OutboxDataBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public IServiceCollection Services { get; }

    public void UseCustomRepository<TRepository>() where TRepository : class, IOutboxRepository
    {
        Services.AddScoped<TRepository>();
        Services.AddScoped<IOutboxRepository>(provider => provider.GetService<TRepository>() ?? throw new UnreachableException());
    }
}