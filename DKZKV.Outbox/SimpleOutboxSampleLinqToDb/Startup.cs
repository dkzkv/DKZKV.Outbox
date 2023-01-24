using DKZKV.Outbox;
using LinqToDB.AspNet;
using LinqToDB.Configuration;
using MB.Outbox.Persistence.LinqToDb;
using MB.Outbox.Persistence.LinqToDb.MsSql;
using SimpleOutboxSampleLinqToDb.Persistence;

namespace SimpleOutboxSampleLinqToDb;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection") 
                               ?? throw new ApplicationException("Connections string is not defined");
        
        services.AddLinqToDBContext<AppDataConnection>((_, options) => { options.UseSqlServer(connectionString); });
        services.AddOutboxProvider(builder => builder.UseLinq2Db<AppDataConnection>(o => o.UseSqlServer()));

        services.AddHostedService<ExampleOrderSaver>();
    }
        
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
    }
}