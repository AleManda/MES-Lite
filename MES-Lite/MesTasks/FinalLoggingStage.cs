using MES_Lite.MesEntities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MES_Lite.MesTasks
{
    internal class FinalLoggingStage: IFinalLoggingStage
    {
        private readonly ILogger<ValidationStage> _logger;

        public FinalLoggingStage(ILogger<ValidationStage> logger)
        {
            _logger = logger;
        }

        public async Task RunAsync(ChannelReader<MaterialDefinition> input)
        {
            await foreach (var matdef in input.ReadAllAsync())
            {
                // Simulate final logging logic
                _logger.LogInformation($"FINAL---Final Log: Material {matdef.Id} with Batch {matdef.BatchId} is ready for processing.");
            }
        }
    }
}
