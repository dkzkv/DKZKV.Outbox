using Serilog;
using SimpleOutboxSampleLinqToDb;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

CreateHostBuilder(args).Build().Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseSerilog() 
        .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });