using MES_Lite.Data;
using MES_Lite.MesChannels;
using MES_Lite.MesEntities;
using MES_Lite.MesTasks;
using Microsoft.EntityFrameworkCore;
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

