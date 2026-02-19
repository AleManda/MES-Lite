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
    internal class BatchAssignmentStage:IBatchAssignmentStage
    {
        private readonly ILogger<ValidationStage> _logger;

        public BatchAssignmentStage(ILogger<ValidationStage> logger)
        {
            _logger = logger;
        }
        public async Task RunAsync(ChannelReader<MaterialDefinition> input, ChannelWriter<MaterialDefinition> output)
        {

            await foreach (var matdef in input.ReadAllAsync())
            {
                // Simulate batch assignment logic
                matdef.BatchId = $"LOT-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4]}";

                _logger.LogInformation($"BATCH---Material {matdef.Id} assigned to {matdef.BatchId}.");
                await output.WriteAsync(matdef);
            }
            output.Complete();
        }
    }
}
