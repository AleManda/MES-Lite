using MES_Lite.Data;
using MES_Lite.MesChannels;
using MES_Lite.MesEntities;
using MES_Lite.MesTasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.Threading.Channels;


var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config =>
    {
        config.SetBasePath(Directory.GetCurrentDirectory()); // Imposta la root
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((hostContext,services) =>
    {

        //Database context
        var connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<MesLiteDbContext>(options => options.UseSqlServer(connectionString));


        //workers
        services.AddSingleton<IMaterialDefinitionGenerator, MaterialDefinitionGenerator>();
        services.AddSingleton<IValidationStage, ValidationStage>();
        services.AddSingleton<IBatchAssignmentStage, BatchAssignmentStage>();
        services.AddSingleton<IFinalLoggingStage, FinalLoggingStage>();






        //Channels
        //services.AddSingleton(Channel.CreateBounded<MaterialDefinition>(new BoundedChannelOptions(50)
        //{
        //    FullMode = BoundedChannelFullMode.Wait
        //}));
        //services.AddSingleton(Channel.CreateBounded<MaterialDefinition>(new BoundedChannelOptions(50)
        //{
        //    FullMode = BoundedChannelFullMode.Wait
        //}));
        //services.AddSingleton(Channel.CreateBounded<MaterialDefinition>(new BoundedChannelOptions(50)
        //{
        //    FullMode = BoundedChannelFullMode.Wait
        //}));

        //Using wrapper classes for channels to allow for better DI and potential future enhancements
        services.AddSingleton<InputChannel>();
        services.AddSingleton<ValidatedChannel>();
        services.AddSingleton<AssignedChannel>();

        //Orchestrator
        services.AddSingleton<IPipelineOrchestrator, PipelineOrchestrator>();

        //Logging
        services.AddLogging(builder =>
        {
            builder.AddSimpleConsole(options =>
            {
                options.SingleLine = true;
                options.TimestampFormat = "HH:mm:ss ";
            });
        });





    })
    .Build();

// Start the pipeline orchestrator
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
await host.Services.GetRequiredService<IPipelineOrchestrator>().RunAsync(cts.Token);




// This factory is used by EF Core tools to create the DbContext at design time (e.g., for migrations)
// It is not used at runtime, but it is necessary for EF Core to work properly with our DbContext
//FinalLoggingStage is a singleton and depends on a scoped dbcontext, so it will be shared across the application,
//but EF Core tools need a way to create an instance of the DbContext without going through
//the full DI setup, hence this factory.EF cannot create migrations if you have a singleton that depends on a
//scoped service, so we need to provide a way for EF to create the DbContext without resolving the singleton.
//FinalLoggingStage will be shared across the application, but EF Core tools need a way to create an instance of
//the DbContext without going through the full DI setup, hence this factory.
//just define a class that implements IDesignTimeDbContextFactory and creates the DbContext with the same connection
//string as the application.EF Core tools will automatically use this factory to create the DbContext when running
//commands like "Add-Migration" or "Update-Database" in the Package Manager Console, allowing it to work properly
//even with our singleton that depends on a scoped service.
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;

//namespace MES_Lite.Data
//{
//    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
//    {
//        public AppDbContext CreateDbContext(string[] args)
//        {
//            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

//            optionsBuilder.UseSqlServer(
//                "Server=localhost;Database=MESLite;Trusted_Connection=True;TrustServerCertificate=True;");

//            return new AppDbContext(optionsBuilder.Options);
//        }
//    }
//}
