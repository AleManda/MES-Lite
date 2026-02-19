using MES_Lite.MesChannels;
using MES_Lite.MesEntities;
using MES_Lite.MesTasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.ComponentModel;
using System.Threading.Channels;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
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


        services.AddSingleton<InputChannel>();
        services.AddSingleton<ValidatedChannel>();
        services.AddSingleton<AssignedChannel>();

        //Orchestrator
        services.AddSingleton<IPipelineOrchestrator, PipelineOrchestrator>();

    })
    .Build();

await host.Services.GetRequiredService<IPipelineOrchestrator>().RunAsync();

